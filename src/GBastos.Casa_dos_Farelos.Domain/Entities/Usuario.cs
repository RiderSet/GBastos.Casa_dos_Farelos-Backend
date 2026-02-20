using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events.Usuarios;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Usuario : AggregateRoot
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public string Login { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;
    public string Perfil { get; private set; } = "Funcionario";

    private Usuario() { }

    public Usuario(string login, string senha, string perfil)
    {
        Login = login;
        SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        Perfil = perfil;
    }

    public bool ValidarSenha(string senha)
        => BCrypt.Net.BCrypt.Verify(senha, SenhaHash);

    public void DefinirRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role inválida");

        role = role.Trim();

        if (Perfil == role)
            return;

        Perfil = role;

        AddDomainEvent(new UsuarioRoleAlteradaEvent(Id, role));
    }

    protected override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(Login))
            throw new InvalidOperationException("Login inválido.");

        if (string.IsNullOrWhiteSpace(SenhaHash))
            throw new InvalidOperationException("SenhaHash inválido.");
    }
}