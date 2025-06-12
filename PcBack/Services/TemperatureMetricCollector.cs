using System;
using System.Collections.Generic;
using OpenHardwareMonitor.Hardware;
using PcMetrics.Core.Services.DTO;

namespace PcMetrics.Core.Services
{
    public class TemperatureMetricCollector : IMetricCollector
    {
        private readonly OpenHardwareMonitor.Hardware.Computer _ohmComputer;

        public TemperatureMetricCollector()
        {
            _ohmComputer = new OpenHardwareMonitor.Hardware.Computer();
            _ohmComputer.Open(false); 
        }

        public IEnumerable<CollectedMetricValue> Collect()
        {
            foreach (var hardware in _ohmComputer.Hardware)
            {
                hardware.Update();

                foreach (var sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        yield return new CollectedMetricValue
                        {
                            MetricName = $"{hardware.Name} Temperature",
                            Value = (decimal)Math.Round(sensor.Value ?? 0, 2),
                            RecordedAt = DateTime.Now
                        };
                    }
                }
            }
        }
    }
}