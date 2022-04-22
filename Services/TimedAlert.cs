﻿using LiquidationDashboard.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiquidationDashboard.Services
{
    public class TimedAlert : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly double _jobTimer = 60;

        public TimedAlert(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        private async void DoWork(object state)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var apiService = scope.ServiceProvider.GetRequiredService<IApiService>();
            var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
            var symbolService = scope.ServiceProvider.GetRequiredService<ISymbolService>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var alerts = await userService.GetAlerts();

            foreach (var alert in alerts)
            {
                var storage = await storageService.GetStorage(alert.Address);

                if (storage != null)
                {
                    if (storage.Health > alert.AlertLimit)
                    {
                        Console.WriteLine($"alert Send to {alert.Email} for {alert.Address}");
                        alert.IsSend = true;
                        await userService.UpdateAlert(alert);
                    }
                }
            }
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