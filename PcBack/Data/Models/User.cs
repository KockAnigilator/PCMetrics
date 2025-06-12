using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcMetrics.Core.Data.Models
{
    /// <summary>
    /// Класс-модель для таблицы в бд
    /// </summary>
    public class User
    {
        private int id;
        private string userName;
        private string passwordHash;
        private string role;
        private DateTime createdAt;

        private List<MetricValue> metricValues;

        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string PasswordHash { get => passwordHash; set => passwordHash = value; }
        public string Role { get => role; set => role = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public List<MetricValue> MetricValues { get => metricValues; set => metricValues = value; }
    }
}
