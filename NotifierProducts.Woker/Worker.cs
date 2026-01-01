using NotifierProducts.Application.UseCases;

namespace NotifierProducts.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ProcessProductUseCase _processProductUseCase;

        public Worker(
            ILogger<Worker> logger,
            ProcessProductUseCase processProductUseCase)
        {
            _logger = logger;
            _processProductUseCase = processProductUseCase;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _processProductUseCase.ExecuteAsync();
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Operation was canceled. Worker is stopping.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred in the worker loop.");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}