﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcMetrics.Core.Services.DTO
{
    /// <summary>
    /// Отделяем логику от БД
    /// </summary>
    public class CollectedMetricValue
    {
        public string MetricName { get; set; }
        public decimal Value { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
