using PcBack.Services.Collectors;
using PcMetrics.Core.Data.Repository;
using PcMetrics.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agent
{
    public partial class MonitoringService: ServiceBase
    {
        private Timer _timer;
        private readonly IMetricRepository _metricRepo;
        private readonly IComputerRepository _computerRepo;
        private readonly SystemMonitorService _monitor;
        public MonitoringService()
        {
            InitializeComponent();
            var connString = "Host=localhost;Username=postgres;Password=1545;Database=PcMonitoringDb";
            _metricRepo = new MetricRepository(connString);
            _computerRepo = new ComputerRepository(connString);

            var computer = _computerRepo.GetAll().First();
            _monitor = new SystemMonitorService(_metricRepo, _computerRepo, 5000, computer.Id)
;       }

        protected override void OnStart(string[] args)
        {

            EventLog.WriteEntry("Служба мониторинга запущена", EventLogEntryType.Information);

            _monitor.AddCollector(new CPUMetricCollector());
            _monitor.AddCollector(new RAMMetricCollector());
            _monitor.AddCollector(new NetworkUsageMetricCollector());
            _monitor.AddCollector(new TemperatureMetricCollector());
            _monitor.AddCollector(new DiskUsageMetricCollector());

            _monitor.Start();
        }
 
        protected override void OnStop()
        {
            Console.WriteLine("🔴 Служба мониторинга остановлена");
            _monitor.Stop();
        }
    }
}
