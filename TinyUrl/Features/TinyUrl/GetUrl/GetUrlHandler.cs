namespace TinyUrl.Features.TinyUrl.GetUrl;

public class GetUrlHandler : IGetUrlHandler
{
    private Dictionary<string, string> _urls = new Dictionary<string, string>
    {
        {"AAA", "https://google.com"}
    };
    
    public Task<IResult> HandleAsync(string id, CancellationToken ct)
    {
        if (!_urls.ContainsKey(id))
        {
            return Task.FromResult(Results.NotFound($"No record found matching {id}"));
        }
        
        var matchingRecord = _urls[id];
        return Task.FromResult(Results.Redirect(matchingRecord));
    }
}
