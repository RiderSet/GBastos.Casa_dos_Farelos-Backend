using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.ValueObjects;

public sealed class StatusPagamento : ValueObject
{
    public static readonly StatusPagamento Pendente = new("PENDENTE");
    public static readonly StatusPagamento Aprovado = new("APROVADO");
    public static readonly StatusPagamento Recusado = new("RECUSADO");
    public static readonly StatusPagamento Cancelado = new("CANCELADO");

    public string Value { get; }

    private StatusPagamento(string value)
    {
        Value = value;
    }

    public static StatusPagamento From(string value)
    {
        return value.ToUpperInvariant() switch
        {
            "PENDENTE" => Pendente,
            "APROVADO" => Aprovado,
            "RECUSADO" => Recusado,
            "CANCELADO" => Cancelado,
            _ => throw new ArgumentException($"Status de pagamento inválido: {value}")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}