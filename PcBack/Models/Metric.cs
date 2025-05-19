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
    public class Metric
    {
        private int id;
        private string name;
        private List<MetricValue> metricValues;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public List<MetricValue> MetricValues { get => metricValues; set => metricValues = value; }
    }
}
