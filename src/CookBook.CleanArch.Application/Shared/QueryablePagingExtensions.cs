namespace CookBook.CleanArch.Application.Shared;

public static class QueryablePagingExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingOptions pagingOptions)
    {
        var pageIndex = pagingOptions.PageIndex;
        var pageSize = pagingOptions.PageSize;
        
        return query
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
