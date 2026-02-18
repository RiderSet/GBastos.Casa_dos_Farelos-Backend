namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public record ClientePJCreateDto(
    string RazaoSocial,
    string NomeFantasia,
    string Telefone,
    string Email,
    string CNPJ,
    string Contato
);