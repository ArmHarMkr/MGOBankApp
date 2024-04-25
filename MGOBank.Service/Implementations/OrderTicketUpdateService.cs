using MGOBankApp.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGOBank.Service.Implementations
{
    public class OrderTicketUpdateService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _provider;
        private Timer _timer;

        public OrderTicketUpdateService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateOrderTickets, null, TimeSpan.Zero, TimeSpan.FromSeconds(240));
            return Task.CompletedTask;
        }

        private void UpdateOrderTickets(object state)
        {
            using (var scope = _provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderTicketUpdateService>>();

                var orders = db.OrderTickets.ToList();
                foreach (var order in orders)
                {
                    db.Remove(order);
                }
                logger.LogInformation("Data deleted successfully");
                db.SaveChanges();
            }
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
