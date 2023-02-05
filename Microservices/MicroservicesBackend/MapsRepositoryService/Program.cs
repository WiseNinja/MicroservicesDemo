using Infrastructure;
using MapsRepositoryService.Infrastructure;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRepositoryInfrastructureServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Configure the HTTP request pipeline.
WebApplication app = builder.Build();
app.UseCors();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
