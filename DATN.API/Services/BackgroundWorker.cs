using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATN.DAL.Models;
using System.Threading;

namespace DATN.API.Services
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly IBackgroundQueue<AddJob> _queue;
        private readonly IServiceScopeFactory _scopeFactory;

        public BackgroundWorker(IBackgroundQueue<AddJob> queue, IServiceScopeFactory scopeFactory)
        {
            _queue = queue;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await BackgroundProcessing(stoppingToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {

            return base.StopAsync(cancellationToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(500, stoppingToken);
                    var job = _queue.Dequeue();

                    if (job == null) continue;

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var capNhat = scope.ServiceProvider.GetRequiredService<ICapNhatTuDien>();

                        await capNhat.CapNhat(job, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
