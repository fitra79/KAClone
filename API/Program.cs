using API;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Persistence.SeederRunner;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<SeederRunner>();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception e)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "Error during migration");
}

if (args.Contains("--seed"))
{
    using var seederScope = app.Services.CreateScope(); 
    var seederRunner = seederScope.ServiceProvider.GetRequiredService<SeederRunner>();
    await seederRunner.RunAsync();

    return;
}

app.Run();

