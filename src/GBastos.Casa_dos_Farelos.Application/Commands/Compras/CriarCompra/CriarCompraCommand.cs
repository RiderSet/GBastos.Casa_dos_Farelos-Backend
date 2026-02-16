using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.CriarCompra;

public sealed record CriarCompraCommand(
    Guid FornecedorId,
    List<CriarCompraItemDto> Itens
) : IRequest<Guid>;
