namespace GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

public class ItemCompraDto
{
    public Guid ProdutoId { get; set; }
    public string NomeProduto { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal CustoUnitario { get; set; }
    public decimal SubTotal { get; set; }
}