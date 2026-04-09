using CookBook.CleanArch.Domain;
using MediatR;

namespace CookBook.CleanArch.Application.Abstraction;

public interface ICommandBase;

public interface ICommand : IRequest<Result>, ICommandBase;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result> where TCommand : ICommand;
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>;
