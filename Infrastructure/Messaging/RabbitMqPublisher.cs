using System.Text;
using Application.Interfaces;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public class RabbitMqPublisher : IMessageBusPublisher
{
    public void Publish(string message, string routingKey)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare("quiz_exchange", ExchangeType.Direct, durable: true);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(
            exchange: "quiz_exchange",
            routingKey: routingKey,
            basicProperties: null,
            body: body
        );
    }
}