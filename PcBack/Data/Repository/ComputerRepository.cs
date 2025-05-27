using Dapper;
using Npgsql;
using PcBack.Data.DbContext;
using PcBack.Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcBack.Data.Repository
{
    /// <summary>
    /// Дефолтные операции CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IComputerRepository<T> where T : class
    {
        IEnumerable<T> GetBookList(); // получение всех объектов
        T GetBook(int id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(int id); // удаление объекта по id
    }

    /// <summary>
    /// Реализация репозитория для работы с компьютерами через Dapper
    /// </summary>
    public class ComputerRepository
    {
        private readonly IDbConnection _db;

        /// <summary>
        /// Конструктор для подключения к бд
        /// </summary>
        /// <param name="connectionString">строка подключения к бд</param>
        public ComputerRepository(string connectionString)
        {
            _db = new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// Реализация методов из интерфейс
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Computer> GetAll()
        {
            return _db.Query<Computer>("SELECT * FROM computers");
        }

        /// <summary>
        /// Получить по айди
        /// </summary>
        /// <param name="id"> айди </param>
        /// <returns></returns>
        public Computer GetById(int id)
        {
            return _db.QueryFirstOrDefault<Computer>(
                $"SELECT * FROM computers WHERE id = {1}", id
                );
        }

        /// <summary>
        /// Создание
        /// </summary>
        /// <param name="computer"></param>
        public void Create(Computer computer)
        {
            const string sql = @"
                INSERT INTO Computers (Name, IpAddress, OsVersion, LastSeen)
                VALUES (@Name, @IpAddress, @OsVersion, @LastSeen)";

            _db.Execute(sql, computer);
        }

        /// <summary>
        /// Update опрерация
        /// </summary>
        /// <param name="computer"></param>
        public void Update(Computer computer)
        {
            const string sql = @"
                UPDATE Computers
                SET Name = @Name,
                    IpAddress = @IpAddress,
                    OsVersion = @OsVersion,
                    LastSeen = @LastSeen
                WHERE Id = @Id";

            _db.Execute(sql, computer);
        }

        public void Delete(int id)
        {
            _db.Execute("DELETE FROM computers WHERE Id = @Id", new { Id = id });
        }





    }
}
