using CookBook.Clean.Core.Shared.Exceptions;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests;

public class ImageUrlValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_ImageUrl_With_Valid_Http_Url_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("http://a.png");
        Assert.Equal("http://a.png", url.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Valid_Https_Url_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("https://a.jpg");
        Assert.Equal("https://a.jpg", url.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Protocol_Relative_Url_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("//a.gif");
        Assert.Equal("//a.gif", url.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Complex_Path_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("https://example.com/images/photo.jpeg");
        Assert.Equal("https://example.com/images/photo.jpeg", url.Value);
    }
    
    [Fact]
    public void Creating_ImageUrl_With_Uppercase_Extension_Should_Succeed()
    {
        var url = ImageUrl.CreateObject("https://a.PNG");
        Assert.Equal("https://a.PNG", url.Value);
    }
    
    
    //invalid ones:
    [Fact]
    public void Creating_ImageUrl_Without_Double_Slash_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
                ImageUrl.CreateObject("http:a.png")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_Unsupported_Extension_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            ImageUrl.CreateObject("https://a.bmp")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_No_Extension_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            ImageUrl.CreateObject("https://example.com/image")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_Invalid_Protocol_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            ImageUrl.CreateObject("ftp://a.png")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_Quotes_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            ImageUrl.CreateObject("https://exa\"mple.com/a.png")
        );
    }
    
    [Fact]
    public void Creating_ImageUrl_With_Query_String_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            ImageUrl.CreateObject("https://a.png?size=large")
        );
    }
    
    
}
