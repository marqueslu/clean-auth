using MediatR;

namespace MyMoney.Application.UseCases.Common;

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : IRequest<TResult>;