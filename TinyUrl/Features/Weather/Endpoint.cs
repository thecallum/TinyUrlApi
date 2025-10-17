namespace TinyUrl.Features.Weather;

public class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/weatherforecast", async (IGetWeatherHandler handler, CancellationToken ct) =>
            {
                return await handler.HandleAsync(ct);
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();
    }
}