namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public record CompraDto(
    Guid CompraId,
    Guid FornecedorId,
    decimal TotalCompra,
    List<CompraItemDto> Itens
);