namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public sealed record ClientePFDto(
    Guid Id,
    string Nome,
    string CPF,
    string Telefone,
    string Email,
    DateTime DataNascimento
);