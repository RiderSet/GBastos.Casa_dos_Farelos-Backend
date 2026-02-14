namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public record ClienteDto(
    Guid Id,
    string Nome,
    string Telefone,
    string Email,
    string Tipo,
    string Documento,
    string? Contato = null
);