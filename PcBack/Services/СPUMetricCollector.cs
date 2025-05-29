using PcBack.Services.DTO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcBack.Services
{
    public class СPUMetricCollector : IMetricCollector
    {
        private readonly PerformanceCounter cpuCounter = new PerformanceCounter("Proccesor", "% Proccesor Time", "_Total");

        
        public IEnumerable<CollectedMetricValue> Collect()
        {
            var value = new CollectedMetricValue
            {
                MetricName = "CPU Usage",
                Value = Convert.ToDecimal(cpuCounter.NextValue()),
                RecordedAt = DateTime.Now
            };

            Task.Delay(500).Wait();
            value.Value = Convert.ToDecimal(cpuCounter.NextValue());

            return new List<CollectedMetricValue> { value };
        }

    }
}
