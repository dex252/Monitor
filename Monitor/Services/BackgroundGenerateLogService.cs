using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.Services
{
    public class BackgroundGenerateLogService : BackgroundService
    {
        private bool isActive = true;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    if (!isActive)
                    {
                        Console.WriteLine($"Процесс остановлен: {isActive}");
                        continue;
                    }
                    // Просто ждем 5 секунд и проверяем не запрошена ли отмена


                    // Можно добавить лог для демонстрации, что служба жива
                    Console.WriteLine($"Процесс работает: {isActive}");
                }
                catch (OperationCanceledException)
                {
                    //приложение останавливается
                    await Task.Delay(5000, stoppingToken);
                    break;
                }
                catch (Exception)
                {

                    await Task.Delay(5000, stoppingToken);
                }
                finally
                {
                    Console.WriteLine($"FINAL: {isActive}");
                    await Task.Delay(10000, stoppingToken);
                }
            }


        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }

        public bool Start()
        {
            if (isActive)
            {
                return false;
            }

            isActive = true;
            return true;
        }

        public bool Stop()
        {
            if (!isActive)
            {
                return false;
            }

            isActive = false;
            return true;
        }

        public bool Status()
        {
            return isActive;
        }
    }
}
