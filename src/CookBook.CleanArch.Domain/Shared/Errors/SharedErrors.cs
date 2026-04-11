namespace CookBook.CleanArch.Domain.Shared.Errors;

public static class SharedErrors
{
    public static Error NullImageUrlError() => new("Image URL cannot be null");
    public static Error InvalidImageUrlFormatError(string value) => new($"Invalid image URL format : {value}");
    public static Error InvalidImageUrlExtensionError(string value) => new($"Invalid image URL extension : {value}");

}

