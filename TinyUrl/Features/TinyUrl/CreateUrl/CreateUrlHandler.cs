using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Results;
using TinyUrl.Infrastructure;
using SimpleBase;
using Flurl;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace TinyUrl.Features.TinyUrl.CreateUrl;

public class CreateTinyUrlRequestValidator : AbstractValidator<CreateTinyUrlRequest>
{
    public CreateTinyUrlRequestValidator()
    {
        RuleFor(x => x.FullUrl)
            .MinimumLength(3)
            .MaximumLength(100)
            .Must(url => 
            {
                if (string.IsNullOrWhiteSpace(url)) return false;
        
                try 
                { 
                    var flurl = Url.Parse(url);
                    return flurl.Scheme == "http" || flurl.Scheme == "https";
                }
                catch { return false; }
            })
            .WithMessage("Must be a valid HTTP or HTTPS URL");
    }
}

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
            Console.WriteLine(validationErrors);
            
        if (validationErrors.Any())
        {
            return Results.ValidationProblem(validationErrors.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray()));
        }
        
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
    
    private List<ValidationFailure> ValidateRequest(CreateTinyUrlRequest request)
    {
        var _validator = new CreateTinyUrlRequestValidator();
        
        var result =  _validator.Validate(request);

        if (!result.IsValid)
        {
            return result.Errors;
        }

        return new List<ValidationFailure>();
    }
}