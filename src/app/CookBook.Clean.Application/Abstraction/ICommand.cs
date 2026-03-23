using CookBook.Clean.Core;
using MediatR;

namespace CookBook.Clean.Application.Abstraction;

public interface ICommandBase;

public interface ICommand : IRequest<Result>, ICommandBase;
public interface IQuery<TResponse> : IRequest<Result>, ICommandBase;
