namespace CookBook.CleanArch.Application.Queries;

public static class QueryablePagingExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingOptions? pagingOptions)
    {
        if (pagingOptions is null)
            return query;

        return query
            .Skip(pagingOptions.PageSize * pagingOptions.PageIndex)
            .Take(pagingOptions.PageSize);
    }
}
