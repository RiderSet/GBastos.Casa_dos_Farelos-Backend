namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed class VendaDetalhadaDto
{
    public Guid Id { get; init; }
    public DateTime DataVenda { get; init; }

    public Guid ClienteId { get; init; }
    public string Cliente { get; init; } = string.Empty;

    public Guid FuncionarioId { get; init; }
    public string Funcionario { get; init; } = string.Empty;

    public decimal Total { get; init; }

    public List<VendaItemDto> Itens { get; init; } = [];
}
