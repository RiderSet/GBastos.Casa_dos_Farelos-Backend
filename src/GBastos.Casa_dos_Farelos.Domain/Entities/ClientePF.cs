using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePF : Cliente
{
    public string CPF { get; private set; } = string.Empty;
    public DateTime DtNascimento { get; private set; }

    private ClientePF() { }

    private ClientePF(
        string nome,
        string telefone,
        string email,
        string cpf,
        DateTime dtNascimento)
        : base(nome, telefone, email)
    {
        SetDocumento(cpf);
        SetDtNascimento(dtNascimento);
    }

    public static ClientePF CriarClientePF(
        string nome,
        string telefone,
        string email,
        string cpf,
        DateTime dtNascimento)
    {
        return new ClientePF(nome, telefone, email, cpf, dtNascimento);
    }

    public void AtualizarClientePF(
        string nome,
        string telefone,
        string email,
        string cpf,
        DateTime dtNascimento)
    {
        Atualizar(nome, telefone, email);
        SetDocumento(cpf);
        SetDtNascimento(dtNascimento);
    }

    // ===================== REGRAS =====================

    private void SetDocumento(string cpf)
    {
        cpf = NormalizarCpf(cpf);

        if (!CpfValido(cpf))
            throw new ArgumentException("CPF inválido", nameof(cpf));

        CPF = cpf;
    }

    private void SetDtNascimento(DateTime dtNascimento)
    {
        if (dtNascimento == default)
            throw new ArgumentException("Data de nascimento obrigatória", nameof(dtNascimento));

        var hoje = DateTime.UtcNow.Date;

        if (dtNascimento.Date > hoje)
            throw new ArgumentException("Data futura inválida", nameof(dtNascimento));

        var idade = hoje.Year - dtNascimento.Year;
        if (dtNascimento.Date > hoje.AddYears(-idade)) idade--;

        if (idade < 0 || idade > 130)
            throw new ArgumentException("Data de nascimento inválida", nameof(dtNascimento));

        DtNascimento = dtNascimento.Date;
    }

    // ===================== HELPERS =====================

    private static string NormalizarCpf(string cpf)
        => new string((cpf ?? "").Where(char.IsDigit).ToArray());

    private static bool CpfValido(string cpf)
    {
        if (cpf.Length != 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;

        int CalcularDigito(string str, int[] peso)
        {
            var soma = str.Select((c, i) => (c - '0') * peso[i]).Sum();
            var resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        int[] peso1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] peso2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var dig1 = CalcularDigito(cpf[..9], peso1);
        var dig2 = CalcularDigito(cpf[..9] + dig1, peso2);

        return cpf.EndsWith($"{dig1}{dig2}");
    }

    protected override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(Email))
            throw new DomainException("Email é obrigatório.");

        if (string.IsNullOrWhiteSpace(CPF))
            throw new DomainException("CPF é obrigatório.");

        if (!CpfValido(CPF))
            throw new DomainException("CPF inválido.");

        if (DtNascimento == default)
            throw new DomainException("Data de nascimento obrigatória.");

        var hoje = DateTime.UtcNow.Date;

        if (DtNascimento > hoje)
            throw new DomainException("Data de nascimento futura inválida.");

        var idade = hoje.Year - DtNascimento.Year;
        if (DtNascimento > hoje.AddYears(-idade)) idade--;

        if (idade < 0 || idade > 130)
            throw new DomainException("Data de nascimento inválida.");
    }
}