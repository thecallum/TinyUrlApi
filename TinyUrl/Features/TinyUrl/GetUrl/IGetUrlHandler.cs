namespace TinyUrl.Features.TinyUrl.GetUrl;

public interface IGetUrlHandler
{
    Task<IResult> HandleAsync(string id, CancellationToken ct);
}