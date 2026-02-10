namespace GBastos.Casa_dos_Farelos.Application.Dtos
{
    public sealed class CompraResumoDto
    {
        public Guid Id { get; init; }
        public DateTime DataCompra { get; init; }
        public decimal Total { get; init; }
        public Guid FornecedorId { get; init; }
    }
}
