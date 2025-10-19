using FluentValidation;
using Flurl;

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