using Amazon;
using Amazon.Runtime;
using Amazon.SQS;

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

builder.Services.AddHostedService<CheckPrices.Worker.Worker>();

builder.Build().Run();
