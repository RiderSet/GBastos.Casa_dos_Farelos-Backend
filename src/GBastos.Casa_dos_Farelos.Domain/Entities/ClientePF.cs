namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePF : Pessoa
{
    public string CPF { get; private set; } = string.Empty;
    public DateTime DtNascimento { get; private set; }

    private ClientePF() { } // EF

    private ClientePF(
        string nome,
        string telefone,
        string email,
        string cpf,
        DateTime dtNascimento)
        : base(nome, telefone, email)
    {
        DefinirDocumento(cpf);
        DefinirNascimento(dtNascimento);
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
        DefinirDocumento(cpf);
        DefinirNascimento(dtNascimento);
    }

    // ===================== REGRAS =====================

    private void DefinirDocumento(string cpf)
    {
        cpf = NormalizarCpf(cpf);

        if (!CpfValido(cpf))
            throw new ArgumentException("CPF inválido", nameof(cpf));

        CPF = cpf;
    }

    private void DefinirNascimento(DateTime dtNascimento)
    {
        if (dtNascimento == default)
            throw new ArgumentException("Data de nascimento obrigatória", nameof(dtNascimento));

        if (dtNascimento > DateTime.UtcNow.Date)
            throw new ArgumentException("Data de nascimento inválida", nameof(dtNascimento));

        if (DateTime.UtcNow.Year - dtNascimento.Year > 130)
            throw new ArgumentException("Data de nascimento inválida", nameof(dtNascimento));

        DtNascimento = dtNascimento.Date;
    }

    // ===================== HELPERS =====================

    private static string NormalizarCpf(string cpf)
        => new string(cpf.Where(char.IsDigit).ToArray());

    private static bool CpfValido(string cpf)
    {
        if (cpf.Length != 11) return false;
        if (cpf.All(c => c == cpf[0])) return false;

        int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var temp = cpf[..9];
        int soma = temp.Select((t, i) => (t - '0') * mult1[i]).Sum();
        int resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        temp += resto;
        soma = temp.Select((t, i) => (t - '0') * mult2[i]).Sum();
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(resto.ToString());
    }
}