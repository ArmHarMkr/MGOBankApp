using MGOBankApp.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            _timer = new Timer(UpdateOrderTickets, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void UpdateOrderTickets(object state)
        {
            using (var scope = _provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var orders = db.OrderTickets.Where(o => o.IsDone).ToList();

                foreach (var order in orders)
                {
                    order.IsDone = false;
                }

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
