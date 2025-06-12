using System;
using System.Linq;
using PcMetrics.Core.Data.Repository;
using PcMetrics.Core.Services;

namespace PcMetrics.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 🔐 Строка подключения к твоей БД
            var connString = "Host=localhost;Username=postgres;Password=1545;Database=PcMonitoringDb";

            // 💾 Репозитории
            IComputerRepository computerRepo = new ComputerRepository(connString);
            IMetricRepository metricRepo = new MetricRepository(connString);

            // 🖥 Получаем или создаём компьютер
            var computer = computerRepo.GetAll().FirstOrDefault();

            if (computer == null)
            {
                Console.WriteLine("❌ Нет доступных компьютеров в БД");
                return;
            }

            Console.WriteLine($"✅ Выбран компьютер: {computer.Name} (ID: {computer.Id})");

            // 🚦 Создаём и настраиваем мониторингSystem.MissingMethodException: "Метод не найден: "Void OpenHardwareMonitor.Hardware.Computer.set_CPUEnabled
            var monitor = new SystemMonitorService(metricRepo, computerRepo, intervalMs: 5000, computer.Id);

            // 🔌 Добавляем сборщики метрик
            monitor.AddCollector(new CPUMetricCollector());
            monitor.AddCollector(new RAMMetricCollector());
            monitor.AddCollector(new TemperatureMetricCollector());
            monitor.AddCollector(new DiskUsageMetricCollector());
            monitor.AddCollector(new NetworkUsageMetricCollector());

            // ▶️ Запускаем мониторинг
            monitor.Start();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();

            // ⏹ Останавливаем сервис
            monitor.Stop();
            Console.WriteLine("Мониторинг остановлен.");
        }
    }
}