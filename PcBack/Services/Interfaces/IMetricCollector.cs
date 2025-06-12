using PcMetrics.Core.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcMetrics.Core.Services
{
    /// <summary>
    /// Интерфейс по сбору метрик с ПК
    /// </summary>
    public interface IMetricCollector
    {
        IEnumerable<CollectedMetricValue> Collect();
    }
}
