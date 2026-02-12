using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed class CompraCriadaDomainEvent : DomainEvent
{
    public Guid CompraId { get; }
    public Guid FornecedorId { get; }

    public CompraCriadaDomainEvent(Guid compraId, Guid fornecedorId)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
    }
}
