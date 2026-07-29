namespace Otex.BuildingBlocks.Application.Cqrs;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
