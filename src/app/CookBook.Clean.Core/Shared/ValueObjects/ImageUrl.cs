using System.Text.RegularExpressions;

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
            ? Result.Invalid<ImageUrl>($"Invalid image URL format : {value}")
            : Result.Ok(new ImageUrl(value));
    }

    public static implicit operator string(ImageUrl name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
