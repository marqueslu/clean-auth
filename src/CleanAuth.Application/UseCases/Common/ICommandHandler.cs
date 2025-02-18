using MediatR;

namespace CleanAuth.Application.UseCases.Common;

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : IRequest<TResult>;