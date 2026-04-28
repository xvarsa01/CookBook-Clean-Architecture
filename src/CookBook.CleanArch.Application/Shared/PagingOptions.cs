using System.ComponentModel;

namespace CookBook.CleanArch.Application.Shared;

public class PagingOptions
{
    [DefaultValue(0)]
    public int PageIndex
    {
        get => field < 0 ? 0 : field;
        set;
    }

    [DefaultValue(10)]
    public int PageSize
    {
        get => field <= 0 ? 10 : field;
        set;
    } = 10;
}
