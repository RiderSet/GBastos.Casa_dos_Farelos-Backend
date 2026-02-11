namespace GBastos.Casa_dos_Farelos.Infrastructure.Security;

public static class SecretProvider
{
    public static string? TryGet(string key)
    {
        // 1) Environment Variable (Cloud first)
        var env = Environment.GetEnvironmentVariable(key);
        if (!string.IsNullOrWhiteSpace(env))
            return env;

        // 2) Docker Swarm Secret
        var path = $"/run/secrets/{key}";
        if (File.Exists(path))
            return File.ReadAllText(path).Trim();

        return null;
    }

    public static string GetRequired(string key)
    {
        var value = TryGet(key);

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Secret '{key}' não encontrado em ENV nem Docker Secrets.");

        return value;
    }
}
