namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public sealed class FornecedorProdutoDto
{
    public Guid ProdutoId { get; set; }
    public string Produto { get; set; } = default!;
    public Guid FornecedorId { get; set; }
    public string Fornecedor { get; set; } = default!;
}
