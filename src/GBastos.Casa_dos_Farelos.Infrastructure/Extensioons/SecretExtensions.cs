namespace GBastos.Casa_dos_Farelos.Infrastructure.Extensioons;

public static class SecretExtensions
{
    public static string ReadSecret(string name)
    {
        var path = $"/run/secrets/{name}";
        return File.Exists(path)
            ? File.ReadAllText(path).Trim()
            : throw new Exception($"Docker secret '{name}' não encontrado.");
    }
}
