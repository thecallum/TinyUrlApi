using TinyUrl;
using TinyUrl.Features.TinyUrl.CreateUrl;
using TinyUrl.Features.TinyUrl.GetUrl;
using TinyUrl.Features.Weather;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddSingleton<IGetWeatherHandler, GetWeatherHandler>();
builder.Services.AddSingleton<IGetUrlHandler, GetUrlHandler>();
builder.Services.AddSingleton<ICreateUrlHandler, CreateUrlHandler>();




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