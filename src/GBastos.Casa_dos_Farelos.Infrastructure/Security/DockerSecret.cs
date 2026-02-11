namespace GBastos.Casa_dos_Farelos.Infrastructure.Security;

public static class DockerSecrets
{
    public static string? TryRead(string name)
    {
        // 1️⃣ Environment variable (Azure / Kubernetes / CI)
        var env = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrWhiteSpace(env))
            return env;

        // 2️⃣ Docker Swarm secret file
        var path = $"/run/secrets/{name}";
        if (File.Exists(path))
            return File.ReadAllText(path).Trim();

        // 3️⃣ Não encontrado
        return null;
    }

    public static string ReadRequired(string name)
    {
        var value = TryRead(name);

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Secret '{name}' não encontrado. Configure Docker Secret ou variável de ambiente.");

        return value;
    }
}
