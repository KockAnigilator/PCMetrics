using PcBack.Data.Models;
using PcBack.Data.Repository;
using System;

namespace PcMetrics.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var connString = "Host=localhost;Username=postgres;Password=1545;Database=PcMonitoringDb";

            ComputerRepository computerRepository = new ComputerRepository(connString);
            MetricRepository metricRepository = new MetricRepository(connString);

            Console.WriteLine("=== Тестирование базы данных ===\n");

            // Получаем список всех компьютеров
            Console.WriteLine("Компьютеры:");
            foreach (var comp in computerRepository.GetAll())
            {
                Console.WriteLine($"ID: {comp.Id}, Имя: {comp.Name}, IP: {comp.IpAddress}");
            }

            // Получаем список всех метрик
            Console.WriteLine("\nМетрики:");
            foreach (var metric in metricRepository.GetAllMetrics())
            {
                Console.WriteLine($"ID: {metric.Id}, Название: {metric.Name}");
            }

            // Получаем CPU метрику
            var cpu = metricRepository.GetMetricByName("CPU Usage");
            if (cpu != null)
            {
                Console.WriteLine($"\nПоследние значения CPU (метрика ID={cpu.Id}):");
                var values = metricRepository.GetLastMetricValues(cpu.Id, 5);
                foreach (var val in values)
                {
                    Console.WriteLine($"{val.RecordedAt} → {val.Value}%");
                }

                // Добавляем новое значение метрики
                var testValue = new MetricValue
                {
                    ComputerId = 1,
                    MetricId = cpu.Id,
                    Value = 45.5m,
                    RecordedAt = DateTime.Now
                };

                metricRepository.CreateMetricValue(testValue);
                Console.WriteLine("\n✅ Новое значение CPU успешно добавлено!");
            }
            else
            {
                Console.WriteLine("\n❌ Метрика 'CPU Usage' не найдена в БД.");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();

        }
    }
}
