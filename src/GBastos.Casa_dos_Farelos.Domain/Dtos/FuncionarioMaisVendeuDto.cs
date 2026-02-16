namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed class FuncionarioMaisVendeuDto
{
    public Guid FuncionarioId { get; set; }
    public string Nome { get; set; } = default!;
    public decimal TotalVendido { get; set; }
    public int QuantidadeVendas { get; set; }
}
