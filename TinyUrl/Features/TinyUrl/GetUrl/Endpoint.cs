namespace TinyUrl.Features.TinyUrl.GetUrl;

public class Endpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/url/{id}", async (string id, IGetUrlHandler handler, CancellationToken ct) =>
            {
                return await handler.HandleAsync(id, ct);
            })
            .WithName("GetTinyUrl")
            .WithOpenApi();
    }
}