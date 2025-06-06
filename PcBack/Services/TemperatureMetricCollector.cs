using OpenHardwareMonitor.Hardware;
using PcBack.Data.Models;
using PcBack.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcBack.Services
{
    public class TemperatureMetricCollector : IMetricCollector
    {
        private readonly OpenHardwareMonitor.Hardware.Computer _ohmComputer;

        public TemperatureMetricCollector()
        {
            _ohmComputer = new OpenHardwareMonitor.Hardware.Computer
            {
                CPUEnabled = true,
                GPUEnabled = true,
                FanControllerEnabled = false,
                RAMEnabled = false,
                MainboardEnabled = false,
                HDDEnabled = false
            };
            _ohmComputer.Open();
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
