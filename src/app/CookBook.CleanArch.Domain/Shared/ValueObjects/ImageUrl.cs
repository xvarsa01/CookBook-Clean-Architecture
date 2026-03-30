using System.Text.RegularExpressions;
using CookBook.CleanArch.Domain.Shared.Errors;

namespace CookBook.CleanArch.Domain.Shared.ValueObjects;

public class ImageUrl : IValueObject<string>, IValueObjectFactory<ImageUrl, string>
{
    public string Value { get; }

    private static readonly Regex BaseUrlRegex = new(@"^(https?:)?\/\/[^""']+$", RegexOptions.IgnoreCase | RegexOptions.Compiled
    );

    private static readonly Regex ImageExtensionRegex = new(@"\.(png|jpg|jpeg|gif|svg)$", RegexOptions.IgnoreCase | RegexOptions.Compiled
    );
    
    private ImageUrl(string value)
    {
        Value = value;
    }

    public static Result<ImageUrl> CreateObject(string value)
    {
        return Result.Create(value)
            .Ensure(v => !string.IsNullOrWhiteSpace(v), SharedErrors.NullImageUrlError())
            .Ensure(v => BaseUrlRegex.IsMatch(v), SharedErrors.InvalidImageUrlFormatError(value))
            .Ensure(v => ImageExtensionRegex.IsMatch(v), SharedErrors.InvalidImageUrlExtensionError(value))
            .Map(v => new ImageUrl(v));
    }

    public static implicit operator string(ImageUrl name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
