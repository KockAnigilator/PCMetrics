using PcMetrics.Core.Services.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcMetrics.Core.Services
{
    using System.Diagnostics;

    public class NetworkUsageMetricCollector : IMetricCollector
    {
        private readonly PerformanceCounter networkInCounter;
        private readonly PerformanceCounter networkOutCounter;

        public NetworkUsageMetricCollector()
        {
            string instance = GetFirstNetworkInstance();

            networkInCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
            networkOutCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
        }

        private string GetFirstNetworkInstance()
        {
            foreach (var category in PerformanceCounterCategory.GetCategories())
            {
                if (category.CategoryName.Contains("Network Interface"))
                {
                    var instances = category.GetInstanceNames();
                    if (instances.Length > 0)
                    {
                        return instances[0]; // Берём первый активный интерфейс
                    }
                }
            }

            throw new InvalidOperationException("Сетевой интерфейс не найден");
        }

        public IEnumerable<CollectedMetricValue> Collect()
        {
            decimal inSpeedMB = Convert.ToDecimal(networkInCounter.NextValue()) / (1024 * 1024);
            decimal outSpeedMB = Convert.ToDecimal(networkOutCounter.NextValue()) / (1024 * 1024);

            Task.Delay(500).Wait(); // Первое значение может быть некорректным

            inSpeedMB = Convert.ToDecimal(networkInCounter.NextValue()) / (1024 * 1024);
            outSpeedMB = Convert.ToDecimal(networkOutCounter.NextValue()) / (1024 * 1024);

            yield return new CollectedMetricValue
            {
                MetricName = "Network Download (MB/s)",
                Value = Math.Round(inSpeedMB, 2),
                RecordedAt = DateTime.Now
            };

            yield return new CollectedMetricValue
            {
                MetricName = "Network Upload (MB/s)",
                Value = Math.Round(outSpeedMB, 2),
                RecordedAt = DateTime.Now
            };
        }
    }
}