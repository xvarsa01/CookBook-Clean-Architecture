using System.Text.RegularExpressions;
using CookBook.CleanArch.Domain.Shared.Errors;

namespace CookBook.CleanArch.Domain.Shared.ValueObjects;

public class ImageUrl : IValueObject<string>, IValueObjectFactory<ImageUrl, string>
{
    public string Value { get; }

    private static readonly Regex ImageUrlRegex = new(
        @"^(https?:)?(\/\/[^""']*\.(png|jpg|jpeg|gif|svg))$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );
    
    private ImageUrl(string value)
    {
        Value = value;
    }

    public static Result<ImageUrl> CreateObject(string value)
    {
        return (string.IsNullOrWhiteSpace(value) || !ImageUrlRegex.IsMatch(value))
            ? Result.Invalid<ImageUrl>(SharedErrors.InvalidImageUrlFormatError(value))
            : Result.Ok(new ImageUrl(value));
    }

    public static implicit operator string(ImageUrl name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
