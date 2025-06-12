using PcMetrics.Core.Services.DTO;
using PcMetrics.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PcBack.Services.Collectors
{
    public class NetworkUsageMetricCollector : IMetricCollector
    {
        private readonly PerformanceCounter networkInCounter;
        private readonly PerformanceCounter networkOutCounter;

        public NetworkUsageMetricCollector()
        {
            string categoryName = GetNetworkCategoryName();
            string instanceName = GetFirstNetworkInstance(categoryName);

            networkInCounter = new PerformanceCounter(categoryName, "Bytes Received/sec", instanceName);
            networkOutCounter = new PerformanceCounter(categoryName, "Bytes Sent/sec", instanceName);
        }

        // Получаем имя сетевой категории
        private string GetNetworkCategoryName()
        {
            foreach (var category in PerformanceCounterCategory.GetCategories())
            {
                if (category.CategoryName.IndexOf("network", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return category.CategoryName;
                }
            }

            throw new InvalidOperationException("Сетевая категория счетчиков не найдена");
        }

        // Получаем первый подходящий интерфейс
        private string GetFirstNetworkInstance(string categoryName)
        {
            var category = new PerformanceCounterCategory(categoryName);
            string[] instances = category.GetInstanceNames();

            if (instances.Length == 0)
                throw new InvalidOperationException("Нет активных сетевых подключений");

            foreach (string instance in instances)
            {
                // Проверяем, что это не виртуальный/ненужный адаптер
                if (instance.IndexOf("isatap", StringComparison.OrdinalIgnoreCase) < 0 &&
                    instance.IndexOf("loopback", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return instance;
                }
            }

            return instances[0]; // Если все остальные не подходят
        }

        public IEnumerable<CollectedMetricValue> Collect()
        {
                decimal inSpeedMB = Convert.ToDecimal(networkInCounter.NextValue() / (1024 * 1024));
                decimal outSpeedMB = Convert.ToDecimal(networkOutCounter.NextValue() / (1024 * 1024));

                // Первый вызов может быть некорректным — делаем задержку
                System.Threading.Tasks.Task.Delay(500).Wait();

                inSpeedMB = Convert.ToDecimal(networkInCounter.NextValue() / (1024 * 1024));
                outSpeedMB = Convert.ToDecimal(networkOutCounter.NextValue() / (1024 * 1024));

                // ✅ Выносим yield за пределы try-catch
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