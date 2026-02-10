using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto
{
    public sealed class CriarProdutoCommand : IRequest<Guid>
    {
        public string Nome { get; }
        public string Descricao { get; }
        public decimal Preco { get; }
        public Guid CategoriaId { get; }  

        public int QuantEstoque { get; }

        public CriarProdutoCommand(string nome, string descricao, decimal preco, Guid categoriaId, int quantEstoque = 0)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            CategoriaId = categoriaId;
            QuantEstoque = quantEstoque;
        }
    }
}
