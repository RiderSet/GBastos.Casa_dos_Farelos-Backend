namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Produtos.Requests
{
    public sealed class CreateProdutoRequest
    {
        public string Nome { get; init; } = string.Empty;
        public string Descricao { get; init; } = string.Empty;
        public decimal Preco { get; init; }
        public int Estoque { get; init; }
        public Guid CategoriaId { get; init; }

        public CreateProdutoRequest() { }

        public CreateProdutoRequest(string nome, string descricao, decimal preco, int estoque, Guid categoriaId)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            Estoque = estoque;
            CategoriaId = categoriaId;
        }
    }
}
