using PcBack.Data.Models;
using PcBack.Data.Repository;
using PcBack.Services;
using System;
using System.Linq;

namespace PcMetrics.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Host=localhost;Username=postgres;Password=1545;Database=PcMonitoringDb";

            var computerRepo = new ComputerRepository(connString);
            var metricRepo = new MetricRepository(connString);

            var computer = computerRepo.GetAll().FirstOrDefault() ??
                           throw new Exception("Нет доступных компьютеров");

            var monitor = new SystemMonitorService(metricRepo, computerRepo, intervalMs: 5000, computer.Id);

            monitor.AddCollector(new CPUMetricCollector());
            monitor.AddCollector(new RAMMetricCollector());

            monitor.Start();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            monitor.Stop();
        }
    }
}
