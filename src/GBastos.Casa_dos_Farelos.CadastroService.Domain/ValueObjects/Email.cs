
using System.Text.RegularExpressions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.ValueObjects;

public sealed class Email
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Endereco { get; }

    private Email(string endereco)
    {
        Endereco = endereco;
    }

    public static Email Criar(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-mail não pode ser vazio.", nameof(email));

        var normalizado = email.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalizado))
            throw new ArgumentException("E-mail inválido.", nameof(email));

        return new Email(normalizado);
    }

    public override string ToString() => Endereco;

    public override bool Equals(object? obj)
        => obj is Email other && Endereco == other.Endereco;

    public override int GetHashCode()
        => Endereco.GetHashCode();

    public static implicit operator string(Email email)
        => email.Endereco;
}