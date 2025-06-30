namespace Configuration.Application.Common.Interface;

public interface IMessagePublisher
{
    void Publish<T>(T message);
}
