namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public record ClientePJCreateDto(
    string Nome,
    string Telefone,
    string Email,
    string CNPJ,
    string Contato,
    DateTime DtCadastro
);