namespace GBastos.Casa_dos_Farelos.Application.Abstraction;

public interface IEmailService
{
    Task EnviarAsync(
        string email,
        string nome,
        CancellationToken ct);

    Task EnviarAsync(
        string destino,
        string assunto,
        string corpoHtml,
        CancellationToken ct);
}