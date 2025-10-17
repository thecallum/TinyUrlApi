namespace TinyUrl.Features.TinyUrl.CreateUrl;

public class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/url/", async (CreateTinyUrlRequest request, ICreateUrlHandler handler, CancellationToken ct) =>
            {
                return await handler.HandleAsync(request, ct);
            })
            .WithName("CreateTinyUrl")
            .WithOpenApi();
    }
}