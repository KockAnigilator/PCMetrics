using PcBack.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PcBack.Services
{
    /// <summary>
    /// Интерфейс по сбору метрик с ПК
    /// </summary>
    public interface IMetricCollector
    {
        IEnumerable<CollectedMetricValue> Collect();
    }
}
