using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class OutboxDispatcher : IOutboxDispatcher
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(
        IServiceProvider provider,
        ILogger<OutboxDispatcher> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public Task DispatchAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DispatchPendingAsync(CancellationToken ct)
    {
        using var scope = _provider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();

        var messages = await repository.GetPendingAsync(20, ct);

        foreach (var message in messages)
        {
            try
            {
                // publicar no EventBus aqui
                await repository.MarkAsProcessedAsync(message.Id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar Outbox");
                await repository.MarkAsFailedAsync(message.Id, ex.Message, ct);
            }
        }
    }
}