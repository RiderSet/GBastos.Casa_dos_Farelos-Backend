using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePJ : Cliente
{
    public string CNPJ { get; private set; } = string.Empty;
    public string RazaoSocial { get; private set; } = string.Empty;
    public string NomeFantasia { get; private set; } = string.Empty;
    public string Contato { get; private set; } = string.Empty;

    private ClientePJ() { }

    private ClientePJ(
        string razaoSocial,
        string nomeFantasia,
        string telefone,
        string email,
        string cnpj,
        string contato)
        : base(razaoSocial, telefone, email)
    {
        DefinirDadosEmpresa(nomeFantasia, cnpj, contato);
    }

    public static ClientePJ CriarClientePJ(
        string razaoSocial,
        string nomeFantasia,
        string telefone,
        string email,
        string cnpj,
        string contato)
    {
        return new ClientePJ(razaoSocial, nomeFantasia, telefone, email, cnpj, contato);
    }

    public void AtualizarClientePJ(
        string razaoSocial,
        string nomeFantasia,
        string telefone,
        string email,
        string cnpj,
        string contato)
    {
        Atualizar(razaoSocial, telefone, email);
        DefinirDadosEmpresa(nomeFantasia, cnpj, contato);
    }

    // ===================== REGRAS =====================

    private void DefinirDadosEmpresa(string nomeFantasia, string cnpj, string contato)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome fantasia é obrigatório", nameof(nomeFantasia));

        cnpj = NormalizarCnpj(cnpj);

        if (!CnpjValido(cnpj))
            throw new ArgumentException("CNPJ inválido", nameof(cnpj));

        NomeFantasia = nomeFantasia.Trim();
        CNPJ = cnpj;
        Contato = contato?.Trim() ?? "";
    }

    // ===================== HELPERS =====================

    private static string NormalizarCnpj(string cnpj)
        => new string((cnpj ?? "").Where(char.IsDigit).ToArray());

    private static bool CnpjValido(string cnpj)
    {
        if (cnpj.Length != 14) return false;
        if (cnpj.All(c => c == cnpj[0])) return false;

        int CalcularDigito(string str, int[] peso)
        {
            var soma = str.Select((c, i) => (c - '0') * peso[i]).Sum();
            var resto = soma % 11;
            return resto < 2 ? 0 : 11 - resto;
        }

        int[] peso1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] peso2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var dig1 = CalcularDigito(cnpj[..12], peso1);
        var dig2 = CalcularDigito(cnpj[..12] + dig1, peso2);

        return cnpj.EndsWith($"{dig1}{dig2}");
    }

    public override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(RazaoSocial))
            throw new DomainException("Razão social é obrigatória.");

        if (string.IsNullOrWhiteSpace(NomeFantasia))
            throw new DomainException("Nome fantasia é obrigatório.");

        if (string.IsNullOrWhiteSpace(CNPJ))
            throw new DomainException("CNPJ é obrigatório.");

        if (!CnpjValido(CNPJ))
            throw new DomainException("CNPJ inválido.");

        if (CNPJ.Length != 14)
            throw new DomainException("CNPJ deve conter 14 dígitos.");

        // opcional: garantir que não ficou com lixo
        if (CNPJ.Any(c => !char.IsDigit(c)))
            throw new DomainException("CNPJ deve conter apenas números.");
    }
}