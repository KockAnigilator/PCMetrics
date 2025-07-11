﻿using System.Collections.Generic;

namespace PcMetrics.Core.Data.Models
{
    /// <summary>
    /// Класс-модель для таблицы Metrics
    /// </summary>
    public class Metric
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Связь один ко многим: одна метрика может иметь много значенийf
        public List<MetricValue> MetricValues { get; set; }
    }
}