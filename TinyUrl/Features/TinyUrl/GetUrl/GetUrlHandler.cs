using Microsoft.EntityFrameworkCore;
using TinyUrl.Infrastructure;

namespace TinyUrl.Features.TinyUrl.GetUrl;

public class GetUrlHandler : IGetUrlHandler
{
    private readonly TinyUrlDbContext _dbContext;

    public GetUrlHandler(TinyUrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IResult> HandleAsync(string id, CancellationToken ct)
    {
        var matchingRecord = await _dbContext.Urls.FirstOrDefaultAsync(x => x.ShortUrl == id);
        if (matchingRecord is null)
        {
            return Results.NotFound($"No record found matching {id}");
        }

        return Results.Redirect(matchingRecord.FullUrl);
    }
}
