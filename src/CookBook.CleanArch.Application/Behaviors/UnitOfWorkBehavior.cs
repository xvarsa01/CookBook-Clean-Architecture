using CookBook.CleanArch.Application.Abstraction;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain;
using MediatR;

namespace CookBook.CleanArch.Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{
    private readonly ICookBookDbContext _dbContext;

    public UnitOfWorkBehavior(ICookBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        if (response is Result { IsFailure: true })
        {
            // Do not commit transaction when result of command is failure
            return response;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}
