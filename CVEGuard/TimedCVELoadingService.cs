using CVEGuard.Library;
using CVEGuard.Library.Contracts;
using CVEGuard.Model;

namespace CVEGuard
{
    public class TimedCVELoadingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(15); // Run every 1 minute

        public TimedCVELoadingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(_interval);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DoWork();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in Timer Service: {ex.Message}");
                }

                await timer.WaitForNextTickAsync(stoppingToken);
            }
        }

        private async Task DoWork()
        {
            var scope = _serviceProvider.CreateScope();
            var scopedService = scope.ServiceProvider.GetRequiredService<ICVELoaderService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var count = await unitOfWork.CountVulnerabilitiesAsync();
            await scopedService.ExecuteTaskAsync(async (v)=>await Save(v, unitOfWork),count);
        }

        private async Task Save(IList<Vulnerability> vulnerabilities, IUnitOfWork unitOfWork)
        {
            foreach (var vulnerability in vulnerabilities) {
                    await unitOfWork.AddVulnerabilityAsync(vulnerability);
            }
            await unitOfWork.CompleteAsync();
}
    }
}