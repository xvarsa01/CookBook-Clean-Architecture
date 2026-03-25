namespace CookBook.CleanArch.Domain.Shared.Errors;

public static class SharedErrors
{
    public static Error InvalidImageUrlFormatError(string value) => new($"Invalid image URL format : {value}");

}

