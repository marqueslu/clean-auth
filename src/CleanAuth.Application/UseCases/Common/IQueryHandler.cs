using MediatR;

namespace CleanAuth.Application.UseCases.Common;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IRequest<TResult>;
