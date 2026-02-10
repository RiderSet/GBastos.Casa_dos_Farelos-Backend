namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public sealed class VendaDto
{
    public Guid Id { get; init; }
    public DateTime Data { get; init; }
    public string Cliente { get; init; } = string.Empty;

    public List<VendaItemDto> Itens { get; init; } = new();

    public decimal Total => Itens.Sum(i => i.Total);
}