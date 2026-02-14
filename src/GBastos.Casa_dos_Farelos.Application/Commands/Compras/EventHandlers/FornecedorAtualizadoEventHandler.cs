using GBastos.Casa_dos_Farelos.Domain.Events.Fornecedores;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.EventHandlers;

public class FornecedorAtualizadoEventHandler : INotificationHandler<FornecedorAtualizadoDomainEvent>
{
    public Task Handle(FornecedorAtualizadoDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fornecedor atualizado: {notification.FornecedorId} - {notification.Nome}");
        return Task.CompletedTask;
    }
}