using CookBook.CleanArch.Domain.Shared.ValueObjects;

namespace CookBook.CleanArch.DomainTests.ValueObjects;

public class ImageUrlValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_ImageUrl_With_Valid_Http_Url_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("http://a.png");
        Assert.True(url.IsSuccess);
        Assert.Equal("http://a.png", url.Value.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Valid_Https_Url_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("https://a.jpg");
        Assert.True(url.IsSuccess);
        Assert.Equal("https://a.jpg", url.Value.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Protocol_Relative_Url_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("//a.gif");
        Assert.True(url.IsSuccess);
        Assert.Equal("//a.gif", url.Value.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Complex_Path_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("https://example.com/images/photo.jpeg");
        Assert.True(url.IsSuccess);
        Assert.Equal("https://example.com/images/photo.jpeg", url.Value.Value);
    }
    
    [Fact]
    public void Creating_ImageUrl_With_Uppercase_Extension_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("https://a.PNG");
        Assert.True(url.IsSuccess);
        Assert.Equal("https://a.PNG", url.Value.Value);
    }
    
    
    //invalid ones:
    [Fact]
    public void Creating_ImageUrl_Without_Double_Slash_Should_ReturnFailure()
    {
        var url = ImageUrl.CreateObject("http:a.png");
        Assert.True(url.IsFailure);
    }

    [Fact]
    public void Creating_ImageUrl_With_Unsupported_Extension_Should_ReturnFailure()
    {
        var url = ImageUrl.CreateObject("https://a.bmp");
        Assert.True(url.IsFailure);
    }

    [Fact]
    public void Creating_ImageUrl_With_No_Extension_Should_ReturnFailure()
    {
        var url = ImageUrl.CreateObject("https://example.com/image");
        Assert.True(url.IsFailure);
    }

    [Fact]
    public void Creating_ImageUrl_With_Invalid_Protocol_Should_ReturnFailure()
    {
        var url = ImageUrl.CreateObject("ftp://a.png");
        Assert.True(url.IsFailure);
    }

    [Fact]
    public void Creating_ImageUrl_With_Quotes_Should_ReturnFailure()
    {
        var url = ImageUrl.CreateObject("https://exa\"mple.com/a.png");
        Assert.True(url.IsFailure);
    }
    
    [Fact]
    public void Creating_ImageUrl_With_Query_String_Should_ReturnFailure()
    {
        var url = ImageUrl.CreateObject("https://a.png?size=large");
        Assert.True(url.IsFailure);
    }
    
    
}
