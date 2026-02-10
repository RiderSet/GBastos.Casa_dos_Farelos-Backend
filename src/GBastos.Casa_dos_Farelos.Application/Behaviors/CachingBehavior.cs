using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;

    public CachingBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        // só aplica cache em queries marcadas
        if (request is not ICacheableQuery cacheableQuery)
            return await next();

        var cached = await _cache.GetAsync<TResponse>(cacheableQuery.CacheKey, ct);

        if (cached is not null)
            return cached;

        var response = await next();

        await _cache.SetAsync(
            cacheableQuery.CacheKey,
            response,
            cacheableQuery.Expiration,
            ct);

        return response;
    }
}
