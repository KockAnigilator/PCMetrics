using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcBack.Models
{
    /// <summary>
    /// Класс-модель для таблицы в бд
    /// </summary>
    public class Computer
    {
        private int id;
        private string name;
        private string ipAddress;
        private string osVersion;
        private DateTime lastSeen;

        private List<MetricValue> metricValue;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string IpAddress { get => ipAddress; set => ipAddress = value; }
        public string OsVersion { get => osVersion; set => osVersion = value; }
        public DateTime LastSeen { get => lastSeen; set => lastSeen = value; }
        public List<MetricValue> MetricValue { get => metricValue; set => metricValue = value; }
    }
}
