using GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class OutboxBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxBackgroundService> _logger;

    public OutboxBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var dispatcher = scope.ServiceProvider
                .GetRequiredService<IOutboxDispatcher>();

            try
            {
                await dispatcher.DispatchAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no dispatcher");
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}