using LiquidationDashboard.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public class TimedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly double _jobTimer = 60;

        public TimedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        private async void DoWork(object state)
        {
            Console.WriteLine("Timed Service Started");

            using var scope = _serviceScopeFactory.CreateScope();
            var apiService = scope.ServiceProvider.GetRequiredService<IApiService>();
            var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
            var symbolService = scope.ServiceProvider.GetRequiredService<ISymbolService>();


            var symbols = await apiService.GetActiveSymbols();
            foreach (var symbol in symbols)
            {
                await symbolService.Add(symbol);

                Console.WriteLine($"Search for {symbol}");

                var storages = await apiService.GetStorages(symbol);

                Console.WriteLine($"Fined {storages.Count()} items");
                foreach (var storageDto in storages)
                {
                    var storage = new Storage()
                    {
                        Address = storageDto.Address,
                        UserBorrow = storageDto.StorageState.User_global_borrowed_in_dollars,
                        MaxBorrow = storageDto.StorageState.User_global_max_borrow_in_dollars,
                    };
                    await storageService.Add(storage);
                }
            }
            Console.WriteLine("Timed Service End");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_jobTimer));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
