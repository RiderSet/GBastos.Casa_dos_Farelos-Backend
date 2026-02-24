namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public sealed record ReservarEstoqueCommandResponse(
    bool Sucesso,
    string? Mensagem,
    Guid ProdutoId,
    int QuantidadeReservada,
    DateTime DataReserva
);
