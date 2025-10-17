using System.ComponentModel.DataAnnotations;

namespace TinyUrl.Features.TinyUrl.CreateUrl;

public class CreateUrlHandler : ICreateUrlHandler
{
    private Dictionary<string, string> _urls = new Dictionary<string, string>
    {
        {"AAA", "https://google.com"}
    };
    
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
    
    public Task<IResult> HandleAsync(CreateTinyUrlRequest request, CancellationToken ct)
    {
        var validationErrors = ValidateRequest(request);
        if (validationErrors is not null) return Task.FromResult(Results.ValidationProblem(validationErrors));
        
        var nextId = _urls.Count() + 1;
        
        byte[] bytes = BitConverter.GetBytes(nextId);
        string encoded = Convert.ToBase64String(bytes)
            .TrimEnd('=')           // Remove padding
            .Replace('+', '-')      // Make URL-safe
            .Replace('/', '_');     // Make URL-safe
        
        _urls.Add(encoded, request.FullUrl);

        var response = new CreateTinyUrlResponse(encoded, request.FullUrl);

        return Task.FromResult(Results.Created(new Uri($"https://url/{encoded}/"), response));
    }
}