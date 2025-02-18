using MediatR;

namespace MyMoney.Application.UseCases.Common;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IRequest<TResult>;
