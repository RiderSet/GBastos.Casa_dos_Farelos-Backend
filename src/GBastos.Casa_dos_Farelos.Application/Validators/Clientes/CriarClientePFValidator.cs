using FluentValidation;
using GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente;

namespace GBastos.Casa_dos_Farelos.Application.Validators.Clientes;

public sealed class CriarClientePFValidator : AbstractValidator<CriarClientePFCommand>
{
    public CriarClientePFValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("O nome deve ter no mínimo 3 caracteres.");

        RuleFor(x => x.CPF)
            .NotEmpty()
            .Length(11)
            .WithMessage("CPF deve conter 11 dígitos.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("E-mail inválido.");

        RuleFor(x => x.DtNascimento)
            .LessThan(DateTime.Today)
            .WithMessage("Data de nascimento inválida.");
    }
}
