using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Vendas.CriarVenda;

/// <summary>
/// Comando para registrar uma nova venda
/// </summary>
public sealed record CriarVendaCommand(
    Guid ClienteId,
    Guid FuncionarioId,
    List<CriarItemVendaCommand> Itens
) : IRequest<Guid>;
