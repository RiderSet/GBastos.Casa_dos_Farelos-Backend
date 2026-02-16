using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public record CompraDto(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<CompraItemDto> Itens
);