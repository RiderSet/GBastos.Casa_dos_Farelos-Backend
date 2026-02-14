using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Fornecedores;

public sealed class FornecedorAtualizadoDomainEvent : DomainEvent
{
    public Guid FornecedorId { get; }
    public string Nome { get; }
    public string Email { get; }
    public string Telefone { get; }
    public string CNPJ { get; }

    public FornecedorAtualizadoDomainEvent(
        Guid fornecedorId,
        string nome,
        string email,
        string telefone,
        string cnpj)
    {
        FornecedorId = fornecedorId;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        CNPJ = cnpj;
    }
}