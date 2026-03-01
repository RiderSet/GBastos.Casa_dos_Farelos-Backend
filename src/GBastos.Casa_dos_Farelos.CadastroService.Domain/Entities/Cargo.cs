using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;

public sealed class Cargo : Entity<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public bool Ativo { get; private set; }

    private Cargo() : base(Guid.Empty) { } // EF Core

    public Cargo(string nome, string? descricao = null)
        : base(Guid.NewGuid())
    {
        AlterarNome(nome);
        Descricao = descricao;
        Ativo = true;
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cargo é obrigatório.");

        if (nome.Length > 100)
            throw new ArgumentException("Nome do cargo deve ter no máximo 100 caracteres.");

        Nome = nome.Trim();
    }

    public void AlterarDescricao(string? descricao)
    {
        if (descricao?.Length > 500)
            throw new ArgumentException("Descrição deve ter no máximo 500 caracteres.");

        Descricao = descricao?.Trim();
    }

    public void Desativar()
    {
        if (!Ativo)
            return;

        Ativo = false;
    }

    public void Ativar()
    {
        if (Ativo)
            return;

        Ativo = true;
    }
}