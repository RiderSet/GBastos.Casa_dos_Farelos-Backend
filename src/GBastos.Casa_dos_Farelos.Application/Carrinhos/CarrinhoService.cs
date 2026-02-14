

using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Carrinhos;

public class CarrinhoService_ZZZ
{
    private readonly ICarrinhoRepository _repo;

    public CarrinhoService_ZZZ(ICarrinhoRepository repo) => _repo = repo;

    public async Task<Carrinho> ObterOuCriarCarrinho(Guid clienteId)
    {
        var carrinho = await _repo.ObterPorClienteIdAsync(clienteId);
        if (carrinho is null)
        {
            carrinho = new Carrinho(clienteId);
            await _repo.AdicionarAsync(carrinho);
        }
        return carrinho;
    }

    public async Task AdicionarItem(Guid clienteId, Guid produtoId, string nome, decimal preco, int quantidade)
    {
        var carrinho = await ObterOuCriarCarrinho(clienteId);
        carrinho.AdicionarItem(produtoId, nome, preco, quantidade);
        await _repo.AtualizarAsync(carrinho);
    }

    public async Task RemoverItem(Guid clienteId, Guid produtoId)
    {
        var carrinho = await ObterOuCriarCarrinho(clienteId);
        carrinho.RemoverItem(produtoId);
        await _repo.AtualizarAsync(carrinho);
    }

    public async Task LimparCarrinho(Guid clienteId)
    {
        var carrinho = await ObterOuCriarCarrinho(clienteId);
        carrinho.Limpar();
        await _repo.AtualizarAsync(carrinho);
    }
}