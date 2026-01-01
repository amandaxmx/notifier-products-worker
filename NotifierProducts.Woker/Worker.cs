using Amazon.SQS;
using Amazon.SQS.Model;

namespace CheckPrices.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        public Worker(
            IAmazonSQS sqs,
            IConfiguration configuration)
        {
            _sqs = sqs;

            _queueUrl = configuration["AWS:Sqs:QueueUrl"]
                        ?? throw new ArgumentException("QueueUrl não configurada");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var response = await _sqs.ReceiveMessageAsync(
                        new ReceiveMessageRequest
                        {
                            QueueUrl = _queueUrl,
                            MaxNumberOfMessages = 5,
                            WaitTimeSeconds = 20
                        },
                        stoppingToken);

                    foreach (var message in response.Messages)
                    {
                        await _sqs.DeleteMessageAsync(
                            new DeleteMessageRequest
                            {
                                QueueUrl = _queueUrl,
                                ReceiptHandle = message.ReceiptHandle
                            },
                            stoppingToken);

                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
    }
}
