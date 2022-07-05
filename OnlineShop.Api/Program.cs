using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using OnlineShop.Api.Data;
using OnlineShop.Api.Repositories;
using OnlineShop.Api.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);
// Test
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OnlineShopDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy.WithOrigins("https://192.168.1.110:7037", "http://192.168.1.110:7037")
    .AllowAnyMethod().WithHeaders(HeaderNames.ContentType));

app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

app.Run();