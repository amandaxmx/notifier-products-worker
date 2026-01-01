using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail;
using Amazon.SQS;
using NotifierProducts.Application.Interfaces;
using NotifierProducts.Application.UseCases;
using NotifierProducts.Infra.Services;
using NotifierProducts.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IAmazonSQS>(_ =>
{
    var credentials = new BasicAWSCredentials("teste", "teste");

    var config = new AmazonSQSConfig
    {
        ServiceURL = "http://localhost:4566",
        AuthenticationRegion = "us-east-1",
        UseHttp = true
    };

    return new AmazonSQSClient(credentials, config);
});

builder.Services.AddSingleton<IAmazonSimpleEmailService>(_ =>
{
    var credentials = new BasicAWSCredentials("teste", "teste");
    var config = new AmazonSimpleEmailServiceConfig
    {
        ServiceURL = "http://localhost:4566",
        AuthenticationRegion = "us-east-1",
        UseHttp = true
    };

    return new AmazonSimpleEmailServiceClient(credentials, config);
});

builder.Services.AddSingleton<IMessageService, SqsService>();
builder.Services.AddSingleton<IEmailService, SesService>();
builder.Services.AddTransient<ProcessProductUseCase>();

builder.Services.AddHostedService<Worker>();

builder.Build().Run();
