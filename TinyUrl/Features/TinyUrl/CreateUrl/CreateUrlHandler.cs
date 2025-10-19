using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using TinyUrl.Infrastructure;
using SimpleBase;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace TinyUrl.Features.TinyUrl.CreateUrl;

public class CreateUrlHandler : ICreateUrlHandler
{
    private readonly TinyUrlDbContext _dbContext;
    private readonly CreateTinyUrlRequestValidator _validator;

    public CreateUrlHandler(TinyUrlDbContext dbContext)
    {
        _dbContext = dbContext;
        _validator = new CreateTinyUrlRequestValidator();
    }

    public async Task<IResult> HandleAsync(CreateTinyUrlRequest request, CancellationToken ct)
    {
        var result = _validator.Validate(request);

        if (!result.IsValid)
        {
            var errorResponse = result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x =>
                    x.Select(e => e.ErrorMessage).ToArray());
            return Results.ValidationProblem(errorResponse);
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
}