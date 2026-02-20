using GBastos.Casa_dos_Farelos.Application.Abstraction;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Services;

public sealed class EmailService : IEmailService
{
    public Task EnviarAsync(string destino, string assunto, string corpoHtml, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task EnviarAsync(string email, string nome, CancellationToken ct = default)
    {
        // Aqui você pode integrar com SMTP, SendGrid, SES, etc.
        // Por enquanto, vamos apenas simular envio:
        Console.WriteLine($"[EmailService] Enviando email de boas-vindas para {nome} <{email}>");

        return Task.CompletedTask;
    }
}