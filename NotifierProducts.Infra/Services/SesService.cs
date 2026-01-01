using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Configuration;
using NotifierProducts.Application.Interfaces;
using NotifierProducts.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifierProducts.Infra.Services
{
    public class SesService : IEmailService
    {
        private readonly IAmazonSimpleEmailService _ses;
        private readonly string _fromEmail;
        private readonly string _toEmail;

        public SesService(IAmazonSimpleEmailService ses, IConfiguration configuration)
        {
            _ses = ses;
            _fromEmail = configuration["AWS:Ses:FromEmail"]
                         ?? throw new ArgumentException("FromEmail not configured");
            _toEmail = configuration["AWS:Ses:ToEmail"]
                       ?? throw new ArgumentException("ToEmail not configured");
        }

        public async Task SendEmailAsync(Product product)
        {
            var emailRequest = new SendEmailRequest
            {
                Source = _fromEmail,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { _toEmail }
                },
                Message = new Amazon.SimpleEmail.Model.Message
                {
                    Subject = new Content($"Product Update: {product.Name}"),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = $"<h3>Product Information</h3>\n                                                    <p><strong>Id:</strong> {product.Id}</p>\n                                                    <p><strong>Name:</strong> {product.Name}</p>\n                                                    <p><strong>Price:</strong> {product.Price:C}</p>\n                                                    <p><strong>URL:</strong> <a href='{product.Url}'>{product.Url}</a></p>\n                                                    <p><strong>Active:</strong> {product.Active}</p>"
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = $"Product Information\nId: {product.Id}\nName: {product.Name}\nPrice: {product.Price:C}\nURL: {product.Url}\nActive: {product.Active}"
                        }
                    }
                }
            };

            await _ses.SendEmailAsync(emailRequest);
        }
    }
}