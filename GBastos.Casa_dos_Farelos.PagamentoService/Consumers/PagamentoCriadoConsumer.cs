using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Contexts;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Consumers;

public sealed class PagamentoCriadoConsumer : BackgroundService
{
    private const string QueueName = "pagamento-criado";
    private const string DeadLetterExchange = "pagamento-dlx";

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PagamentoCriadoConsumer> _logger;
    private readonly IConfiguration _configuration;

    private IConnection? _connection;
    private IChannel? _channel;

    public PagamentoCriadoConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<PagamentoCriadoConsumer> logger,
        IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

if (_channel is null)
    throw new InvalidOperationException("RabbitMQ channel não foi inicializado.");

var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());

                var message = JsonSerializer
                    .Deserialize<PagamentoCriadoIntegrationEvent>(body);

                if (message is null)
                {
                    await _channel.BasicRejectAsync(ea.DeliveryTag, false);
                    return;
                }

                await HandleMessageAsync(message, stoppingToken);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem.");

                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumerTag: string.Empty,
            noLocal: false,
            exclusive: false,
            arguments: null,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumerTag: string.Empty,
            noLocal: false,
            exclusive: false,
            arguments: null,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("PagamentoCriadoConsumer iniciado.");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

private async Task InitializeRabbitMqAsync()
{
    var factory = new ConnectionFactory
    {
        HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
        UserName = _configuration["RabbitMQ:User"] ?? "guest",
        Password = _configuration["RabbitMQ:Password"] ?? "guest"
    };

    _connection = await factory.CreateConnectionAsync();
    _channel = await _connection.CreateChannelAsync();

    await _channel.ExchangeDeclareAsync(
        exchange: DeadLetterExchange,
        type: ExchangeType.Fanout,
        durable: true);

    await _channel.QueueDeclareAsync(
        queue: QueueName,
        durable: true,
        exclusive: false,
        autoDelete: false,
        arguments: new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", DeadLetterExchange }
        });

    await _channel.BasicQosAsync(
        prefetchSize: 0,
        prefetchCount: 1,
        global: false);
}

    private async Task HandleMessageAsync(
        PagamentoCriadoIntegrationEvent message,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PagamentoDbContext>();

        await using var transaction =
            await context.Database.BeginTransactionAsync(cancellationToken);

        var alreadyProcessed =
            await context.InboxMessages
                .AnyAsync(x => x.Id == message.Id, cancellationToken);

        if (alreadyProcessed)
        {
            _logger.LogWarning("Mensagem {MessageId} já processada.", message.Id);
            return;
        }

        var pagamento = Pagamento.CriarPagamento(
            pedidoId: message.PedidoId,
            clienteId: message.ClienteId,
            valor: message.Valor,
            metodoPagamento: message.MetodoPagamento,
            moeda: message.Moeda
        );

        context.Pagamentos.Add(pagamento);

        context.InboxMessages.Add(new InboxMessage
        {
            Id = message.Id,
            ReceivedOnUtc = DateTime.UtcNow
        });

        await context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        _logger.LogInformation(
            "Pagamento criado com sucesso. PedidoId: {PedidoId}",
            message.PedidoId);
    }

    private static int GetRetryCount(BasicDeliverEventArgs ea)
    {
        if (ea.BasicProperties.Headers is null)
            return 0;

        if (!ea.BasicProperties.Headers.TryGetValue("x-death", out var value))
            return 0;

        var deaths = value as IList<object>;
        if (deaths is null || deaths.Count == 0)
            return 0;

        var death = deaths[0] as Dictionary<string, object>;
        if (death is null || !death.TryGetValue("count", out var count))
            return 0;

        return Convert.ToInt32(count);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Encerrando consumidor RabbitMQ...");

        if (_channel is not null)
            await _channel.DisposeAsync();

        if (_connection is not null)
            await _connection.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }
}