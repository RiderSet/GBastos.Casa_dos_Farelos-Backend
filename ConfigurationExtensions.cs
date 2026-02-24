using System;

public static class ConfigurationExtensions
{
    public static string GetRequiredConnectionString(
        this IConfiguration configuration,
        string name)
    {
        var value = configuration.GetConnectionString(name);

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"Connection string '{name}' is not configured.");

        return value;
    }
}