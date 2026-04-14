namespace CookBook.CleanArch.Application.Shared;

public static class QueryablePagingExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingOptions? pagingOptions)
    {
        if (pagingOptions is null)
            return query;

        var pageIndex = pagingOptions.PageIndex < 0 ? 0 : pagingOptions.PageIndex;
        var pageSize = pagingOptions.PageSize <= 0 ? 10 : pagingOptions.PageSize;
        
        return query
            .Skip(pageSize * pageIndex)
            .Take(pageSize);
    }
}
