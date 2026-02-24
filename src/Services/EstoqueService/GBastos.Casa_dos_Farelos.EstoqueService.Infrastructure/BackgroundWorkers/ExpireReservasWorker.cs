using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.BackgroundWorkers;

public class ExpireReservasWorker : BackgroundService
{
    private readonly IServiceScopeFactory _factory;

    public ExpireReservasWorker(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _factory.CreateScope();

            var repo = scope.ServiceProvider
                .GetRequiredService<IEstoqueRepository>();

            await repo.ExpireReservasAsync();

            await Task.Delay(60000, stoppingToken);
        }
    }
}