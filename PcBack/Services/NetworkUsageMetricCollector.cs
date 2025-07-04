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

                    Console.WriteLine($"Интерфейс: {interfaceName}");
                    Console.WriteLine($"Bytes Received/sec: {receivedBytes}");
                    Console.WriteLine($"Bytes Sent/sec: {sentBytes}");

                    yield return new CollectedMetricValue
                    {
                        MetricName = "Network Download (MB/s)",
                        Value = Math.Round(receivedBytes / (1024 * 1024), 2),
                        RecordedAt = DateTime.Now
                    };

                    yield return new CollectedMetricValue
                    {
                        MetricName = "Network Upload (MB/s)",
                        Value = Math.Round(sentBytes / (1024 * 1024), 2),
                        RecordedAt = DateTime.Now
                    };
                }
            }
        }
    }
}