using System.Text;
using System.Text.Json;
using EWallet.Domain.Entities;
using EWallet.Infrastructure.Messaging.Settings;
using RabbitMQ.Client;

namespace EWallet.Infrastructure.Messaging
{
    public class RabbitMqEventPublisher : IEventPublisher
    {
        private readonly RabbitMqConnection _connection;
        private readonly RabbitMqSettings _settings;

        public RabbitMqEventPublisher(RabbitMqConnection connection, RabbitMqSettings settings)
        {
            _connection = connection;
            _settings = settings;
        }

        public Task PublishAsync(Guid entityId, DomainEvent @event)
        {
            using var channel = _connection.GetConnection().CreateModel();

            DeclareTopology(channel);

            var envelope = CreateEnvelope(entityId, @event);
            var body = SerializeEnvelope(envelope);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Type = @event.GetType().Name;

            channel.BasicPublish(
                exchange: _settings.ExchangeName,
                routingKey: @event.GetType().Name,
                basicProperties: properties,
                body: body);

            return Task.CompletedTask;
        }

        private void DeclareTopology(IModel channel)
        {
            channel.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Fanout,
                durable: true);
        }

        private static EventEnvelope CreateEnvelope(Guid entityId, DomainEvent @event)
        {
            return new EventEnvelope
            {
                EntityId = entityId,
                EventId = @event.Id,
                EventType = @event.EventType,
                EventData = JsonSerializer.Serialize(@event, @event.GetType()),
                OccurredOn = @event.EventDate
            };
        }

        private static byte[] SerializeEnvelope(EventEnvelope envelope)
        {
            var json = JsonSerializer.Serialize(envelope);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
