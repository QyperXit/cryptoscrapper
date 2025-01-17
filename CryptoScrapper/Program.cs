using CryptoScrapper.Persistence.Contexts;
using CryptoScrapper.Services.CoinDataService;
using CryptoScrapper.Services.ScrapeCoinService;
using CryptoScrapper.Services.ScrapeCoinService.Factories;
using CryptoScrapper.Services.ScrapeCoinService.Parsers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(10, 5, 23)) // Ensure this matches your MariaDB version
    ));
// add service
// builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICoinService, CoinService>();
builder.Services.AddTransient<ICryptoDataService, CryptoDataService>();
builder.Services.AddScoped<ICryptoHttpClientFactory, CryptoHttpClientFactory>();
builder.Services.AddScoped<CoinParser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();