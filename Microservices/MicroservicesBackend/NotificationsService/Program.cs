using Infrastructure;
using NotificationsService;
using NotificationsService.Hubs;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MessagesHandler>();
builder.Services.AddInfrastructureServices();
builder.Services.AddSignalR();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddCors();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials

app.MapHub<MapEntitiesHub>("/mapEntitiesHub");
app.UseSerilogRequestLogging();

MessagesHandler messagesHandler = app.Services.GetRequiredService<MessagesHandler>();
await messagesHandler.SubscribeAsync();

app.Run();