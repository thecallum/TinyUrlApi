namespace TinyUrl.Features.Weather;

public interface IGetWeatherHandler
{
    Task<IResult> HandleAsync(CancellationToken ct);
}