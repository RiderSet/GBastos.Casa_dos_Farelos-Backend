namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public record ClientePFCreateDto(
    string Nome,
    string Telefone,
    string Email,
    string CPF,
    DateTime DtNascimento
);