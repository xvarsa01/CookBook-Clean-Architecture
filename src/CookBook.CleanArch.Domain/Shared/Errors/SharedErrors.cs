namespace CookBook.CleanArch.Domain.Shared.Errors;

public static class SharedErrors
{
    public static Error NullImageUrlError() =>
        new("SharedErrors.NullImageUrlError", "Image URL cannot be null");

    public static Error InvalidImageUrlFormatError(string value) =>
        new("SharedErrors.InvalidImageUrlFormatError",
            $"Invalid image URL format : {value}", value);

    public static Error InvalidImageUrlExtensionError(string value) =>
        new("SharedErrors.InvalidImageUrlExtensionError",
            $"Invalid image URL extension : {value}", value);
}

