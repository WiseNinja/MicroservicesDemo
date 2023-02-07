using Connectivity.Infrastructure;
using MapsRepositoryService.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddPersistence();
builder.Services.AddConnectivity();
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Configure the HTTP request pipeline.
var app = builder.Build();
app.UseCors();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
