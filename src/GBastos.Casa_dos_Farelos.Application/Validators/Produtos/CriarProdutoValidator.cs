using FluentValidation;
using GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto;

namespace GBastos.Casa_dos_Farelos.Application.Validators.Produtos;

public sealed class CriarProdutoValidator : AbstractValidator<CriarProdutoCommand>
{
    public CriarProdutoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome do produto é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter no mínimo 3 caracteres")
            .MaximumLength(150);

        RuleFor(x => x.Descricao)
            .MaximumLength(500);

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("Categoria é obrigatória");

        RuleFor(x => x.QuantEstoque)
            .GreaterThanOrEqualTo(0).WithMessage("Estoque não pode ser negativo");
    }
}
