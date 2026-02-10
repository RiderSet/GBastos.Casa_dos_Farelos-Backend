using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Usuario : Entity
{
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
}