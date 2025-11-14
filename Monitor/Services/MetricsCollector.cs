using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prometheus;

namespace Monitor.Services
{
    public static class MetricsCollector
    {
        /// <summary>
        /// Метрика для статуса isActive
        /// </summary>
        public static readonly Gauge ServiceStatus = Metrics
            .CreateGauge("monitor_service_active_status", "Background service active status (1 = active, 0 = inactive)");

        /// <summary>
        /// Метрика для ошибок по типам (с двумя лейблами)
        /// </summary>
        public static readonly Counter ErrorsByType = Metrics
            .CreateCounter("monitor_errors_total", "Total number of errors by type and application",
                new CounterConfiguration
                {
                    LabelNames = new[] { "error_type", "application" }
                });
                
        /// <summary>
        /// Метрика для общего числа ошибок
        /// </summary>
        public static readonly Counter TotalErrors = Metrics
            .CreateCounter("monitor_total_errors", "Total number of all errors");

    }
}