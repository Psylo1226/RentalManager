using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RentalManager.Domain.Contracts;
using RentalManager.Infrastructure.Persistance;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,     
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information 
    )
    .WriteTo.File(
        path: "Logs/error-.txt",
        rollingInterval: RollingInterval.Day,    
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error 
    )
    .CreateLogger();

builder.Host.UseSerilog();
var corsPolicy = "RentalManager";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddCors(o => o.AddPolicy(corsPolicy,
    corsPolicyBuilder => { corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
builder.Services.AddDbContext<RentalManagerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRentalManagerUnitOfWork, RentalManagerUnitOfWork>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();