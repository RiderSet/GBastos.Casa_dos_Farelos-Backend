using GBastos.Casa_dos_Farelos.PedidoService.Application.Commands.CriarPedido;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.SharedKernel.Persistence;

namespace GBastos.Casa_dos_Farelos.PedidoService.Application.ConfirmarPedido;

public class CriarPedidoHandler
{
    private readonly IRepository<Pedido, Guid> _repository;
    private readonly IUnitOfWork _uow;

    public CriarPedidoHandler(
        IRepository<Pedido, Guid> repository,
        IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<Guid> Handle(CriarPedidoCommand command)
    {
        var pedido = Pedido.Criar(command.ClienteId);

        await _repository.AddAsync(pedido);
        await _uow.SaveChangesAsync();

        return pedido.Id;
    }
}