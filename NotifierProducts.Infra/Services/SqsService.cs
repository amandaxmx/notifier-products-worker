using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using NotifierProducts.Application.Interfaces;
using NotifierProducts.Application.Models;
using NotifierProducts.Domain.Model;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotifierProducts.Infra.Services
{
    public class SqsService : IMessageService
    {
        private readonly IAmazonSQS _sqs;
        private readonly string _queueUrl;

        public SqsService(IAmazonSQS sqs, IConfiguration configuration)
        {
            _sqs = sqs;
            _queueUrl = configuration["AWS:Sqs:QueueUrl"]
                        ?? throw new ArgumentException("QueueUrl not configured");
        }

        public async Task<List<ProductMessage>> ReceiveMessagesAsync()
        {
            var response = await _sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 5,
                WaitTimeSeconds = 20
            });

            if (response.Messages.Count > 0)
            {
            }

            var productMessages = new List<ProductMessage>();
            foreach (var message in response.Messages)
            {
                try
                {
                    var product = JsonSerializer.Deserialize<Product>(message.Body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (product != null)
                    {
                        productMessages.Add(new ProductMessage
                        {
                            Product = product,
                            ReceiptHandle = message.ReceiptHandle
                        });
                    }
                    else
                    {
                    }
                }
                catch (JsonException jsonEx)
                {
                }
            }

            return productMessages;
        }

        public async Task DeleteMessageAsync(string receiptHandle)
        {
            await _sqs.DeleteMessageAsync(new DeleteMessageRequest
            {
                QueueUrl = _queueUrl,
                ReceiptHandle = receiptHandle
            });

        }
    }
}
