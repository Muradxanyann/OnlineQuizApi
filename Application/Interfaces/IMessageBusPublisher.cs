namespace Application.Interfaces;

public interface IMessageBusPublisher
{
    void Publish(string message, string routingKey);
}