namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed class TotalCompradoClienteDto
{
    public Guid ClienteId { get; set; }
    public string Nome { get; set; } = default!;
    public decimal TotalComprado { get; set; }
}
