using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Utils;

public sealed class Cpf : ValueObject
{
    public string Numero { get; }

    private Cpf(string numero)
    {
        Numero = numero;
    }

    public static Cpf Criar(string cpf)
    {
        if (!CpfValidator.Validar(cpf))
            throw new ArgumentException("CPF inválido");

        var numeroLimpo = new string(cpf.Where(char.IsDigit).ToArray());

        return new Cpf(numeroLimpo);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Numero;
    }
}