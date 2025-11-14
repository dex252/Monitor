using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class BackgroundGenerateLogService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Просто ждем 5 секунд и проверяем не запрошена ли отмена
                    await Task.Delay(5000, stoppingToken);

                    // Можно добавить лог для демонстрации, что служба жива

                }
                catch (OperationCanceledException)
                {
                    // Это нормально - когда приложение останавливается

                    break;
                }
                catch (Exception ex)
                {

                    await Task.Delay(10000, stoppingToken); // Ждем 10 сек перед повторной попыткой
                }
            }


        }
    }
}
