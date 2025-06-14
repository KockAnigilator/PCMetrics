using PcMetrics.Core.Services.DTO;
using PcMetrics.Core.Services;
using System;
using System.Collections.Generic;
using System.Management;

namespace PcBack.Services.Collectors
{
    public class NetworkUsageMetricCollector : IMetricCollector
    {
        public NetworkUsageMetricCollector()
        {
            // В WMI нет необходимости создавать счетчики заранее
            Console.WriteLine("📡 Используется WMI для получения сетевых данных");
        }

        public IEnumerable<CollectedMetricValue> Collect()
        {
            using (var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface"))
            {
                foreach (var queryObj in searcher.Get())
                {
                    string interfaceName = queryObj["Name"]?.ToString() ?? "Unknown";

                    var receivedBytes = Convert.ToDecimal(queryObj["BytesReceivedPerSec"]);
                    var sentBytes = Convert.ToDecimal(queryObj["BytesSentPerSec"]);

                    yield return new CollectedMetricValue
                    {
                        MetricName = $"Network Download ({interfaceName})",
                        Value = Math.Round(receivedBytes / (1024 * 1024), 2),
                        RecordedAt = DateTime.Now
                    };

                    yield return new CollectedMetricValue
                    {
                        MetricName = $"Network Upload ({interfaceName})",
                        Value = Math.Round(sentBytes / (1024 * 1024), 2),
                        RecordedAt = DateTime.Now
                    };
                }
            }
        }
    }
}