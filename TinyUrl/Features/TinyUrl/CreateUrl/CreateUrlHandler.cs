using System.ComponentModel.DataAnnotations;
using TinyUrl.Infrastructure;
using SimpleBase;

namespace TinyUrl.Features.TinyUrl.CreateUrl;

public class CreateUrlHandler : ICreateUrlHandler
{
    private readonly TinyUrlDbContext _dbContext;

    public CreateUrlHandler(TinyUrlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IResult> HandleAsync(CreateTinyUrlRequest request, CancellationToken ct)
    {
        var validationErrors = ValidateRequest(request);
        if (validationErrors is not null) return Results.ValidationProblem(validationErrors);
        
        var newRecord = new Infrastructure.TinyUrl
        {
            FullUrl = request.FullUrl,
            ShortUrl = "placeholder"
        };

        _dbContext.Urls.Add(newRecord);
        await _dbContext.SaveChangesAsync();
        

        var shortUrl = GenerateShortUrl(newRecord.Id);

        newRecord.ShortUrl = shortUrl;
        await _dbContext.SaveChangesAsync();

        var response = new CreateTinyUrlResponse(shortUrl, request.FullUrl);

        return Results.Created(new Uri($"https://url/{shortUrl}/"), response);
    }

    private string GenerateShortUrl(int id)
    {
        var bytes = BitConverter.GetBytes(id);
        return Base62.Default.Encode(bytes);
    }
    
    private Dictionary<string, string[]> ValidateRequest(CreateTinyUrlRequest request)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(request);
        
        var isValid = Validator.TryValidateObject(request, context, validationResults, validateAllProperties: true);

        Console.WriteLine(request);
        Console.WriteLine($"isValid: {isValid}");

        if (!isValid)
        {
            var errors = validationResults
                .GroupBy(x => x.MemberNames.FirstOrDefault() ?? "")
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage ?? "").ToArray());

            return errors;
        }

        return null;
    }
}