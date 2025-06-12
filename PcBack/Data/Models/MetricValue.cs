using System;

namespace PcMetrics.Core.Data.Models
{
    /// <summary>
    /// Класс-модель для таблицы MetricValues
    /// </summary>
    public class MetricValue
    {
        public int Id { get; set; }
        public int ComputerId { get; set; }
        public int MetricId { get; set; }
        public decimal Value { get; set; }
        public DateTime RecordedAt { get; set; }

        // Навигационные свойства
        public Computer Computer { get; set; }
        public Metric Metric { get; set; }
    }
}