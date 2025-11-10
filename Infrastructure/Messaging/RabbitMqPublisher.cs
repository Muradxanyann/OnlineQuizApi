using System.Text;
using System.Text.Json;
using Application.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;


public class RabbitMqPublisher : IMessageBusPublisher
{
    private readonly ConnectionFactory _factory;

    public RabbitMqPublisher()
    {
        _factory = new ConnectionFactory
        {
            HostName = "quiz-rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
    }

    public void Publish<T>(string routingKey, T message)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "quiz_exchange", type: ExchangeType.Direct, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(
            exchange: "quiz_exchange",
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );
    }
}