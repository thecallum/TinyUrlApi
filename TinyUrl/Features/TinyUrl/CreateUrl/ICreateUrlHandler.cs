namespace TinyUrl.Features.TinyUrl.CreateUrl;

public interface ICreateUrlHandler
{
    Task<IResult> HandleAsync(CreateTinyUrlRequest request, CancellationToken ct);
}