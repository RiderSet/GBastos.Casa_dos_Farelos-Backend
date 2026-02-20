namespace GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

public class CompraDto
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public Guid FuncionarioId { get; set; }
    public DateTime DataCompra { get; set; }
    public bool Finalizada { get; set; }
    public List<ItemCompraDto> Itens { get; set; } = new();
}