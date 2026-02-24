using FluentValidation;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public sealed class ReservarEstoqueCommandValidator
    : AbstractValidator<ReservarEstoqueCommand>
{
    public ReservarEstoqueCommandValidator()
    {
        RuleFor(x => x.ProdutoId)
            .NotEmpty()
            .WithMessage("ProdutoId é obrigatório");

        RuleFor(x => x.PedidoId)
            .NotEmpty()
            .WithMessage("PedidoId é obrigatório");

        RuleFor(x => x.Quantidade)
            .GreaterThan(0)
            .WithMessage("Quantidade deve ser maior que zero");

        RuleFor(x => x.IdempotencyKey)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.CorrelationId)
            .NotEmpty();
    }
}