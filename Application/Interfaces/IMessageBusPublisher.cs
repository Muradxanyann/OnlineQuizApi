namespace Application.Interfaces;

public interface IMessageBusPublisher
{
    void Publish<T>(string routingKey, T message);
}