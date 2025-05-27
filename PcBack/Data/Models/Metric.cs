using System.Collections.Generic;

namespace PcBack.Data.Models
{
    /// <summary>
    /// Класс-модель для таблицы Metrics
    /// </summary>
    public class Metric
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Связь один ко многим: одна метрика может иметь много значений
        public List<MetricValue> MetricValues { get; set; }
    }
}