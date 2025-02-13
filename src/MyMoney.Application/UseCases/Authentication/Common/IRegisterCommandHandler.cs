using MediatR;

namespace MyMoney.Application.UseCases.Authentication.Common;

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : IRequest<TResult>;