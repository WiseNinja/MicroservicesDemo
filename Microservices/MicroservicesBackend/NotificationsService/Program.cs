using Common.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationsService;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<MessagesHandler>();
        services.AddInfrastructureServices();
    })
    .Build();

var messagesHandler = host.Services.GetRequiredService<MessagesHandler>();
await messagesHandler.HandleMessagesAsync();
while (true)
{

}