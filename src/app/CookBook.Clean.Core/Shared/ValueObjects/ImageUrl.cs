using System.Text.RegularExpressions;
using CookBook.Clean.Core.Shared.Exceptions;

namespace CookBook.Clean.Core.Shared.ValueObjects;

public class ImageUrl
{
    public string Value { get; }

    private static readonly Regex ImageUrlRegex = new(
        @"^(https?:)?(\/\/[^""']*\.(png|jpg|jpeg|gif|svg))$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );
    
    public ImageUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !ImageUrlRegex.IsMatch(value))
            throw new InvalidImageUrlException(value);
        
        Value = value;
    }

    public static implicit operator string(ImageUrl name) => name.Value;
    
    public override string ToString()
    {
        return Value;
    }
}
