using System.Text.RegularExpressions;
using CookBook.CleanArch.Domain.Shared.Errors;

namespace CookBook.CleanArch.Domain.Shared.ValueObjects;

public record ImageUrl : IValueObject<string>, IValueObjectFactory<ImageUrl, string>
{
    public string Value { get; }

    private static readonly Regex ImageExtensionRegex = new(@"\.(png|jpg|jpeg|gif|svg)$", RegexOptions.IgnoreCase | RegexOptions.Compiled
    );
    
    private static readonly string[] AllowedHosts =
    [
        "images.unsplash.com",
        "picsum.photos"
    ];
    
    private ImageUrl(string value)
    {
        Value = value;
    }

    public static Result<ImageUrl> CreateObject(string value)
    {
        return Result.Create(value)
            .Ensure(v => !string.IsNullOrWhiteSpace(v),
                SharedErrors.NullImageUrlError())
            
            .Ensure(v => Uri.TryCreate(v, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps),
                SharedErrors.InvalidImageUrlFormatError(value))
            
            .Ensure(IsCorrectExtension,
                SharedErrors.InvalidImageUrlExtensionError(value))
            
            .Map(v => new ImageUrl(v));
    }

    public static implicit operator string(ImageUrl name) => name.Value;
    
    private static bool IsCorrectExtension(string v)
    {
        var uri = new Uri(v);
        return ImageExtensionRegex.IsMatch(uri.AbsolutePath) || AllowedHosts.Contains(uri.Host);
    }
}
