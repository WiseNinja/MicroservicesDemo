using Connectivity.Infrastructure;
using NotificationsService;
using NotificationsService.Hubs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<MessagesHandler>();
builder.Services.AddConnectivity();
builder.Services.AddSignalR();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapHub<MapEntitiesHub>("/mapEntitiesHub");
app.UseSerilogRequestLogging();

var messagesHandler = app.Services.GetRequiredService<MessagesHandler>();
await messagesHandler.SubscribeAsync();

app.Run();