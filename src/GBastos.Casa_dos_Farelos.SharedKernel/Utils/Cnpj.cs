using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Utils;

public sealed class Cnpj : ValueObject
{
    public string Numero { get; }

    private Cnpj(string numero)
    {
        Numero = numero;
    }

    public static Cnpj Criar(string cnpj)
    {
        if (!CnpjValidator.Validar(cnpj))
            throw new ArgumentException("CNPJ inválido");

        var numeroLimpo = new string(cnpj.Where(char.IsDigit).ToArray());

        return new Cnpj(numeroLimpo);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Numero;
    }
}