﻿using PcMetrics.Core.Services.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcMetrics.Core.Services
{
    public class DiskUsageMetricCollector : IMetricCollector
    {
        private readonly PerformanceCounter diskCounter =
            new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");

        public IEnumerable<CollectedMetricValue> Collect()
        {
            var value = new CollectedMetricValue
            {
                MetricName = "Disk Usage (%)",
                Value = Convert.ToDecimal(diskCounter.NextValue()),
                RecordedAt = DateTime.Now
            };

            Task.Delay(500).Wait();
            value.Value = Convert.ToDecimal(diskCounter.NextValue());

            yield return value;
        }
    }
}
