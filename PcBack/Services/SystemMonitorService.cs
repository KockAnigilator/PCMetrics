using PcMetrics.Core.Data.Models;
using PcMetrics.Core.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PcMetrics.Core.Services
{
    public class SystemMonitorService
    {

        private readonly IMetricRepository _metricRepository;
        private readonly IComputerRepository _computerRepository;
        private readonly List<IMetricCollector> _collectors;
        private readonly System.Timers.Timer _timer;
        private readonly int _computerId;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="metricRepository">Репозиторий для работы с метриками</param>
        /// <param name="computerRepository">Репозиторий для работы с компьютерами</param>
        /// <param name="intervalMs">Интервал сбора (в миллисекундах)</param>
        /// <param name="computerId">ID компьютера, для которого собираем данные</param>
        public SystemMonitorService(
    IMetricRepository metricRepository,
    IComputerRepository computerRepository,
    int intervalMs,
    int computerId)
        {
            _metricRepository = metricRepository;
            _computerRepository = computerRepository;
            _computerId = computerId;

            _collectors = new List<IMetricCollector>();
            _timer = new Timer(intervalMs);
            _timer.Elapsed += OnTimedEvent;
        }

        /// <summary>
        /// Добавить сборщик метрик
        /// </summary>
        /// <param name="collector"></param>
        public void AddCollector(IMetricCollector collector)
        {
            _collectors.Add(collector);
        }

        /// <summary>
        /// Начать мониторинг
        /// </summary>
        public void Start()
        {
            _timer.Start();
            Console.WriteLine("Мониторинг начался...");
        }

        /// <summary>
        /// Остановить мониторинг
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
            Console.WriteLine("Мониторинг остановлен.");
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"\n Сбор метрик: {e.SignalTime}");

            foreach (var collector in _collectors)
            {
                try
                {
                    var values = collector.Collect();

                    foreach (var val in values)
                    {
                        var metric = _metricRepository.GetMetricByName(val.MetricName);
                        if (metric == null)
                        {
                            Console.WriteLine($"Не найдена метрика: {val.MetricName}");
                            continue;
                        }

                        var dbValue = new MetricValue
                        {
                            ComputerId = _computerId,
                            MetricId = metric.Id,
                            Value = val.Value,
                            RecordedAt = val.RecordedAt
                        };

                        _metricRepository.CreateMetricValue(dbValue);
                        Console.WriteLine($"{val.MetricName}: {val.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сбора: {ex.Message}");
                }
            }
        }
    }
}
