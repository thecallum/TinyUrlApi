using Microsoft.EntityFrameworkCore;
using TinyUrl;
using TinyUrl.Features.TinyUrl.CreateUrl;
using TinyUrl.Features.TinyUrl.GetUrl;
using TinyUrl.Features.Weather;
using TinyUrl.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddTransient<IGetWeatherHandler, GetWeatherHandler>();
builder.Services.AddTransient<IGetUrlHandler, GetUrlHandler>();
builder.Services.AddTransient<ICreateUrlHandler, CreateUrlHandler>();


var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                       ?? "Host=127.0.0.1;Database=postgres;Username=postgres;Password=postgres";

builder.Services.AddDbContext<TinyUrlDbContext>(opt =>
    opt.UseNpgsql(connectionString));


var app = builder.Build();

app.MapEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}