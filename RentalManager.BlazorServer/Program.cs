using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RentalManager.Application.Validators;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;
using RentalManager.Infrastructure.Repositories;
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
builder.Services.AddDbContext<RentalManagerDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IRentalDetailRepository, RentalDetailRepository>();
builder.Services.AddScoped<IRentalManagerUnitOfWork, RentalManagerUnitOfWork>();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
builder.Services.AddScoped<IValidator<ProductType>, ProductTypeValidator>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Logging.ClearProviders();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient());
builder.Services.AddCors(o => o.AddPolicy(corsPolicy, policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RentalManagerDbContext>();
    context.Database.EnsureCreated();
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseCors(corsPolicy);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();