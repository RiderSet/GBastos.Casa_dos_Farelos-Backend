using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface ICarrinhoRepository
{
    Task<Carrinho?> ObterPorClienteIdAsync(Guid clienteId);
    Task AdicionarAsync(Carrinho carrinho);
    Task AtualizarAsync(Carrinho carrinho);
    Task RemoverAsync(Carrinho carrinho);
}