using PcBack.Services.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcBack.Services
{
    public class DiskUsageMetricCollector : IMetricCollector
    {
        private readonly PerformanceCounter diskCounter =
            new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");

        public IEnumerable<CollectedMetricValue> Collect()
        {
            var value = new CollectedMetricValue
            {
                MetricName = "DiskUsage (%)",
                Value = Convert.ToDecimal(diskCounter.NextValue()),
                RecordedAt = DateTime.Now
            };

            Task.Delay(500).Wait();
            value.Value = Convert.ToDecimal(diskCounter.NextValue());

            yield return value;
        }
    }
}
