namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.CriarCompra
{
    public sealed class ItemCompraInput
    {
        public Guid ProdutoId { get; init; }

        public int Quantidade { get; init; }
        public decimal PrecoUnitario { get; init; }
    }
}
