using MediatR;

namespace MyMoney.Application.UseCases.Authentication.Common;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IRequest<TResult>;
