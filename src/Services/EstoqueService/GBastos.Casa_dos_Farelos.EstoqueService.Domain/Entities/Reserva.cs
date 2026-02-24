namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

public sealed class Reserva
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProdutoId { get; private set; }
    public int Quantidade { get; private set; }
    public bool Confirmada { get; private set; }
    public bool Cancelada { get; private set; }
    public DateTime CriadaEm { get; private set; } = DateTime.UtcNow;
    public DateTime ExpiraEm { get; private set; }
    public byte[] RowVersion { get; private set; } = default!;

    private Reserva() { }

    public Reserva(Guid produtoId, int quantidade, TimeSpan tempoExpiracao)
    {
        if (quantidade <= 0)
            throw new ArgumentException("Quantidade deve ser maior que zero.");

        ProdutoId = produtoId;
        Quantidade = quantidade;
        ExpiraEm = DateTime.UtcNow.Add(tempoExpiracao);
    }

    public void Confirmar()
    {
        if (Cancelada)
            throw new InvalidOperationException("Reserva já cancelada.");

        if (Expirada())
            throw new InvalidOperationException("Reserva expirada.");

        Confirmada = true;
    }

    public void Cancelar()
    {
        if (Confirmada)
            throw new InvalidOperationException("Reserva já confirmada.");

        Cancelada = true;
    }

    public bool Expirada()
        => DateTime.UtcNow > ExpiraEm;
}