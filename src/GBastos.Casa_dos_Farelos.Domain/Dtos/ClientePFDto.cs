namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed record ClientePFDto(
    Guid Id,
    string Nome,
    string Telefone,
    string Email,
    string CPF,
    DateTime DtCadastro
);