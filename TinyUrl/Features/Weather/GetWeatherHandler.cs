namespace TinyUrl.Features.Weather;

public class GetWeatherHandler : IGetWeatherHandler
{
    private readonly ILogger<GetWeatherHandler> _logger;

    public GetWeatherHandler(ILogger<GetWeatherHandler> logger)
    {
        _logger = logger;
    }

    public async Task<IResult> HandleAsync(CancellationToken ct)
    {
        _logger.LogInformation("Fetching weather forecast");

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        return Results.Ok(forecast);
    }
}