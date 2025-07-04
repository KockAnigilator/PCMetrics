using PcMetrics.Core.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace PcMetrics.Core.Services
{
    public class RAMMetricCollector : IMetricCollector
    {

        public IEnumerable<CollectedMetricValue> Collect()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                foreach (var os in searcher.Get())
                {
                    var totalMemory = Convert.ToDecimal(os["TotalVisibleMemorySize"]);
                    var freeMemory = Convert.ToDecimal(os["FreePhysicalMemory"]);

                    var usedPercent = ((totalMemory - freeMemory) / totalMemory) * 100;

                    yield return new CollectedMetricValue
                    {
                        MetricName = "RAM Usage (%)",
                        Value = Math.Round(usedPercent, 2),
                        RecordedAt = DateTime.Now
                    };


                    yield return new CollectedMetricValue
                    {
                        MetricName = "RAM Usage (MB)",
                        Value = (totalMemory - freeMemory) / 1024,
                        RecordedAt = DateTime.Now
                    };
                }
            }
        }

    }
}
