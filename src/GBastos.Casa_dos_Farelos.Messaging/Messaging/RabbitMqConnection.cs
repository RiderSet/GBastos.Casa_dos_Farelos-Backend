using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Messaging;

public sealed class RabbitMqConnection : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    private RabbitMqConnection(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public IChannel Channel => _channel;

    public static async Task<RabbitMqConnection> CreateAsync(
        string hostName,
        string userName,
        string password,
        CancellationToken ct = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        // Cria a conexão de forma assíncrona
        var connection = await factory.CreateConnectionAsync(ct);

        // Cria um canal
        var channel = await connection.CreateChannelAsync();

        return new RabbitMqConnection(connection, channel);
    }

    public async Task PublishAsync<T>(
        string queueName,
        T message,
        CancellationToken ct = default)
    {
        // Declara a fila de forma assíncrona
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: ct);

        // Serializa a mensagem em bytes
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        // Propriedades da mensagem (persistent)
        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent // usa enum persistente
        };

        // Publica de forma assíncrona
        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);
    }

    public async Task ConsumeAsync<T>(
        string queueName,
        Func<T, Task> handleMessage,
        CancellationToken ct = default)
    {
        // Declara fila assíncrona
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<T>(body);

            if (message != null)
                await handleMessage(message);

            await _channel.BasicAckAsync(ea.DeliveryTag, false, ct);
        };

        // Inicia consumo assíncrono
        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        _channel.Dispose();
        _connection.Dispose();
    }
}