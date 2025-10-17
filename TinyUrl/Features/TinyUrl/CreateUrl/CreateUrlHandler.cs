using System.ComponentModel.DataAnnotations;
using TinyUrl.Infrastructure;

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
        
        byte[] bytes = BitConverter.GetBytes(newRecord.Id);
        string encoded = Convert.ToBase64String(bytes)
            .TrimEnd('=')           // Remove padding
            .Replace('+', '-')      // Make URL-safe
            .Replace('/', '_');     // Make URL-safe

        newRecord.ShortUrl = encoded;
        await _dbContext.SaveChangesAsync();

        var response = new CreateTinyUrlResponse(encoded, request.FullUrl);

        return Results.Created(new Uri($"https://url/{encoded}/"), response);
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