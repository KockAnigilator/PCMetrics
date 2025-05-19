using System;

namespace PcBack.Models
{
    /// <summary>
    /// Класс-модель для таблицы в бд
    /// </summary>
    public class MetricValue
    {
        private int id;
        private int computerId;
        private int metricId;
        private decimal value;
        private DateTime recordedAt;

        private Computer computer;
        private Metric metric;

        public int Id { get => id; set => id = value; }
        public int ComputerId { get => computerId; set => computerId = value; }
        public int MetricId { get => metricId; set => metricId = value; }
        public decimal Value { get => value; set => this.value = value; }
        public DateTime RecordedAt { get => recordedAt; set => recordedAt = value; }
        public Computer Computer { get => computer; set => computer = value; }
        public Metric Metric { get => metric; set => metric = value; }
    }
}