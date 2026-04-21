using System.ComponentModel;

namespace CookBook.CleanArch.Application.Shared;

public class PagingOptions
{
    [DefaultValue(0)]
    public int PageIndex { get; set; }
    
    [DefaultValue(10)]
    public int PageSize { get; set; }
}
