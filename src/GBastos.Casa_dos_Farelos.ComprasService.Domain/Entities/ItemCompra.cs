using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Entities;

public class ItemCompra : Entity<Guid>
{
    public Guid CompraId { get; private set; }

    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; } = null!;
    public int Quantidade { get; private set; }
    public decimal CustoUnitario { get; private set; }

    public decimal SubTotal => Quantidade * CustoUnitario;

    protected ItemCompra() : base(Guid.Empty){}

    internal ItemCompra(
        Guid compraId,
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal custoUnitario)
        : base(Guid.NewGuid())
    {
        if (compraId == Guid.Empty)
            throw new DomainException("Compra inválida.");

        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new DomainException("Nome do produto é obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (custoUnitario <= 0)
            throw new DomainException("Custo inválido.");

        CompraId = compraId;
        ProdutoId = produtoId;
        NomeProduto = nomeProduto.Trim();
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
    }

    internal void AlterarQuantidade(int novaQuantidade)
    {
        if (novaQuantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        Quantidade = novaQuantidade;
    }
}