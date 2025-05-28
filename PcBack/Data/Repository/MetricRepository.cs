using Dapper;
using Npgsql;
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
    /// Интерфейс для работы с метриками в БД
    /// </summary>
    public interface IMetricRepository
    {
        /// <summary>
        /// Получить все типы метрик (CPU Usage, RAM Usage и т.д.)
        /// </summary>
        IEnumerable<Metric> GetAllMetrics();

        /// <summary>
        /// Получить метрику по ID
        /// </summary>
        Metric GetMetricById(int id);

        /// <summary>
        /// Получить метрику по названию
        /// </summary>
        Metric GetMetricByName(string name);

        /// <summary>
        /// Добавить новую метрику
        /// </summary>
        void CreateMetric(Metric metric);

        /// <summary>
        /// Добавить значение метрики
        /// </summary>
        void CreateMetricValue(MetricValue metricValue);

        /// <summary>
        /// Получить последние N записей по метрике
        /// </summary>
        IEnumerable<MetricValue> GetLastMetricValues(int metricId, int count = 10);
    }


    /// <summary>
    /// CRUD - операции
    /// </summary>
    public class MetricRepository : IMetricRepository
    {
        private readonly IDbConnection _db;

        public MetricRepository(string connectionString)
        {
            _db = new NpgsqlConnection(connectionString);
        }


        /// <summary>
        /// Получить все
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Metric> GetAllMetrics()
        {
            return _db.Query<Metric>("SELECT * FROM metrics");
        }

        /// <summary>
        /// Получить по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Metric GetMetricById(int id)
        {
            return _db.QueryFirstOrDefault<Metric>(
            "SELECT * FROM Metrics WHERE Id = @Id",
            new { Id = id });
        }

        /// <summary>
        /// Получить по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Metric GetMetricByName(string name)
        {
            return _db.QueryFirstOrDefault<Metric>(@"SELECT * FROM metrics WHERE name = @Name", new { Name = name });
        }

        /// <summary>
        /// Создание
        /// </summary>
        /// <param name="metric"></param>
        public void CreateMetric(Metric metric)
        {
            const string sql = @"
                INSERT INTO Metrics (Name)
                VALUES (@Name)";

            _db.Execute(sql, metric);
        }

        /// <summary>
        /// Создание для другой таблице
        /// </summary>
        /// <param name="metricValue"></param>
        public void CreateMetricValue(MetricValue metricValue)
        {
            const string sql = "INSERT INTO metricvalues (computerid, metricid, value, recordedat) VALUES (@ComputerId, @MetricId, @Value, @RecordedAt)";
            _db.Execute(sql, metricValue);
        }

        /// <summary>
        /// Получить последнюю метрику
        /// </summary>
        /// <param name="metricId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<MetricValue> GetLastMetricValues(int metricId, int count = 10)
        {
            var query = $@"
                SELECT * 
                FROM MetricValues 
                WHERE MetricId = @MetricId
                ORDER BY RecordedAt DESC
                LIMIT {count}";

            return _db.Query<MetricValue>(query, new { MetricId = metricId });
        }
    }
}

