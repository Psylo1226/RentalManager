using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using RentalManager.Application.Validators;
using RentalManager.BlazorClient;
using RentalManager.BlazorClient.Services;
using RentalManager.Domain.Contracts;
using RentalManager.Domain.Models;
using RentalManager.Infrastructure.Persistance;
using RentalManager.Infrastructure.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7270/") });
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IProductTypeRepository,ProductTypeRepository>();
builder.Services.AddScoped<IRentalManagerUnitOfWork, RentalManagerUnitOfWork>();
builder.Services.AddDbContext<RentalManagerDbContext>(options => options.UseSqlite("Data Source=RentalManagerDb.db"));
builder.Services.AddScoped<ProductTypeService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
builder.Services.AddScoped<IValidator<ProductType>, ProductTypeValidator>();
await builder.Build().RunAsync();

