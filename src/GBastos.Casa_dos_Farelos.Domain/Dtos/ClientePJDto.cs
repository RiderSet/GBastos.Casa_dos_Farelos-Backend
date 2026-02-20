namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed record ClientePJDto(
    Guid Id,
    string NomeFantasia,
    string Telefone,
    string Email,
    string Contato,
    string CNPJ
);