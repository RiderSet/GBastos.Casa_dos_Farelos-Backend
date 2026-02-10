namespace GBastos.Casa_dos_Farelos.Application.Dtos
{
    public sealed class ItemCompraDto
    {
        public Guid ProdutoId { get; init; }
        public string DescricaoProduto { get; init; } = string.Empty;
        public int Quantidade { get; init; }
        public decimal PrecoUnitario { get; init; }
        public decimal SubTotal { get; init; }
    }
}
