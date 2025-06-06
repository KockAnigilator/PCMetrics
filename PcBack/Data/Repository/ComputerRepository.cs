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
    public interface IComputerRepository
    {
        IEnumerable<Computer> GetAll();
        Computer GetById(int id);
        void Create(Computer item);
        void Update(Computer item);
        void Delete(int id);
    }

    /// <summary>
    /// Реализация репозитория для работы с компьютерами через Dapper
    /// </summary>
    public class ComputerRepository : IComputerRepository
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
        /// Получает список всех компьютеров из базы данных.
        /// </summary>
        /// <returns>Список объектов Computer</returns>
        public IEnumerable<Computer> GetAll()
        {
            return _db.Query<Computer>("SELECT * FROM computers");
        }

        /// <summary>
        /// Получить компьютер по ID
        /// </summary>
        /// <param name="id">ID компьютера</param>
        /// <returns>Объект Computer или null</returns>
        public Computer GetById(int id)
        {
            return _db.QueryFirstOrDefault<Computer>(
                "SELECT * FROM computers WHERE id = @Id",
                new { Id = id });
        }

        /// <summary>
        /// Добавить новый компьютер в БД
        /// </summary>
        /// <param name="computer">Объект Computer</param>
        public void Create(Computer computer)
        {
            const string sql = @"
                INSERT INTO Computers (Name, IpAddress, OsVersion, LastSeen)
                VALUES (@Name, @IpAddress, @OsVersion, @LastSeen)";

            _db.Execute(sql, computer);
        }

        /// <summary>
        /// Обновить данные о компьютере
        /// </summary>
        /// <param name="computer">Объект Computer с новыми данными</param>
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

        /// <summary>
        /// Удалить компьютер по ID
        /// </summary>
        /// <param name="id">ID компьютера</param>
        public void Delete(int id)
        {
            _db.Execute("DELETE FROM computers WHERE Id = @Id", new { Id = id });
        }
    }
}
