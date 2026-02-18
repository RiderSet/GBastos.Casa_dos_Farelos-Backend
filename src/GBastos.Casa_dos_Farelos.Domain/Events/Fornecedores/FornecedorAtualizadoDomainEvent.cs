namespace GBastos.Casa_dos_Farelos.Domain.Events.Fornecedores;

public sealed record FornecedorAtualizadoDomainEvent(Guid FornecedorId, string Nome, string Email, string Telefone, string CNPJ) : DomainEvent;
