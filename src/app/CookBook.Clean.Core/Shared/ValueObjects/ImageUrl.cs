using System.Text.RegularExpressions;
using CookBook.Clean.Core.Shared.Errors;

namespace CookBook.Clean.Core.Shared.ValueObjects;

public class ImageUrl
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
