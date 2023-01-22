using Infrastructure;
using NotificationsService;
using NotificationsService.ExceptionHandling;
using NotificationsService.Hubs;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MessagesHandler>();
builder.Services.AddInfrastructureServices();
builder.Services.AddSignalR();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapHub<MapEntitiesHub>("/mapEntitiesHub");
app.UseSerilogRequestLogging();
app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger>());
MessagesHandler messagesHandler = app.Services.GetRequiredService<MessagesHandler>();
await messagesHandler.SubscribeAsync();

app.Run();