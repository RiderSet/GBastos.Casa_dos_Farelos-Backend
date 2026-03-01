using GBastos.Casa_dos_Farelos.CadastroService.Domain.Enums;
using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.ValueObjects;

public class Documento : ValueObject
{
    public string Numero { get; }
    public TipoDocumento Tipo { get; }

    private Documento(string numero, TipoDocumento tipo)
    {
        Numero = numero;
        Tipo = tipo;
    }

    public static Documento CriarCPF(string cpf)
    {
        return new Documento(cpf, TipoDocumento.CPF);
    }

    public static Documento CriarCNPJ(string cnpj)
    {
        return new Documento(cnpj, TipoDocumento.CNPJ);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Numero;
        yield return Tipo;
    }
}