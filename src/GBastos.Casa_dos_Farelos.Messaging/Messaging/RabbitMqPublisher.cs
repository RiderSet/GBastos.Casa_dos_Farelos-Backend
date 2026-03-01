using RabbitMQ.Client;
using System.Text;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Messaging;

public class RabbitMqPublisher
{
    private readonly IChannel _channel;

    public RabbitMqPublisher(RabbitMqConnection connection)
        => _channel = connection.Channel;

    public async Task PublishAsync(string queueName, string message, CancellationToken ct = default)
    {
        // Declara a fila de forma assíncrona
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: ct);

        var body = Encoding.UTF8.GetBytes(message);

        // Cria propriedades manualmente
        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        // Publica a mensagem de forma assíncrona
        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);
    }
}