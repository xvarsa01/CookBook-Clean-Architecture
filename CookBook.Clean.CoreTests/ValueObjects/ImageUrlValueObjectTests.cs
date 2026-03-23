using CookBook.Clean.Core.Shared.Exceptions;
using CookBook.Clean.Core.Shared.ValueObjects;

namespace CookBook.Clean.CoreTests;

public class ImageUrlValueObjectTests
{
    // valid ones:
    [Fact]
    public void Creating_ImageUrl_With_Valid_Http_Url_Should_Succeed()
    {
        var url = new ImageUrl("http://a.png");
        Assert.Equal("http://a.png", url.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Valid_Https_Url_Should_Succeed()
    {
        var url = new ImageUrl("https://a.jpg");
        Assert.Equal("https://a.jpg", url.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Protocol_Relative_Url_Should_Succeed()
    {
        var url = new ImageUrl("//a.gif");
        Assert.Equal("//a.gif", url.Value);
    }

    [Fact]
    public void Creating_ImageUrl_With_Complex_Path_Should_Succeed()
    {
        var url = new ImageUrl("https://example.com/images/photo.jpeg");
        Assert.Equal("https://example.com/images/photo.jpeg", url.Value);
    }
    
    [Fact]
    public void Creating_ImageUrl_With_Uppercase_Extension_Should_Succeed()
    {
        var url = new ImageUrl("https://a.PNG");
        Assert.Equal("https://a.PNG", url.Value);
    }
    
    
    //invalid ones:
    [Fact]
    public void Creating_ImageUrl_Without_Double_Slash_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
                new ImageUrl("http:a.png")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_Unsupported_Extension_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            new ImageUrl("https://a.bmp")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_No_Extension_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            new ImageUrl("https://example.com/image")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_Invalid_Protocol_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            new ImageUrl("ftp://a.png")
        );
    }

    [Fact]
    public void Creating_ImageUrl_With_Quotes_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            new ImageUrl("https://exa\"mple.com/a.png")
        );
    }
    
    [Fact]
    public void Creating_ImageUrl_With_Query_String_Should_Throw()
    {
        Assert.Throws<InvalidImageUrlException>(() =>
            new ImageUrl("https://a.png?size=large")
        );
    }
    
    
}
