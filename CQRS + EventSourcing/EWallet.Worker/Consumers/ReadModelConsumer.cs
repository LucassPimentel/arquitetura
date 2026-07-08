using System.Text;
using System.Text.Json;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Messaging.Settings;
using EWallet.Infrastructure.Repositories;
using EWallet.Worker.Projections.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EWallet.Worker.Consumers
{
    /// <summary>
    /// Consumidor responsável por hidratar a view materializada (read model).
    /// Despacha cada evento para sua projeção correspondente.
    /// </summary>
    public class ReadModelConsumer : BackgroundService
    {
        private readonly RabbitMqConnection _connection;
        private readonly RabbitMqSettings _settings;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ReadModelConsumer> _logger;
        private readonly Dictionary<string, IEventProjection> _projections;
        private IModel? _channel;

        public ReadModelConsumer(
            RabbitMqConnection connection,
            RabbitMqSettings settings,
            IServiceScopeFactory scopeFactory,
            ILogger<ReadModelConsumer> logger,
            IEnumerable<IEventProjection> projections)
        {
            _connection = connection;
            _settings = settings;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _projections = projections.ToDictionary(p => p.EventType);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel = _connection.GetConnection().CreateModel();

            DeclareTopology();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ProcessMessageAsync;

            _channel.BasicConsume(
                queue: _settings.ReadModelQueueName,
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation(
                "ReadModelConsumer iniciado. Aguardando eventos na fila: {Queue}",
                _settings.ReadModelQueueName);

            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(object sender, BasicDeliverEventArgs args)
        {
            try
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var envelope = JsonSerializer.Deserialize<EventEnvelope>(json);

                if (envelope is null)
                {
                    _logger.LogWarning("Envelope recebido inválido. Mensagem descartada.");
                    _channel?.BasicNack(args.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                await ProjectEventAsync(envelope);

                _channel?.BasicAck(args.DeliveryTag, multiple: false);
                _logger.LogInformation(
                    "Evento {EventType} projetado no Read Model para entidade {EntityId}",
                    envelope.EventType, envelope.EntityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao projetar evento no Read Model. Reenfileirando mensagem.");
                _channel?.BasicNack(args.DeliveryTag, multiple: false, requeue: true);
            }
        }

        private async Task ProjectEventAsync(EventEnvelope envelope)
        {
            if (!_projections.TryGetValue(envelope.EventType, out var projection))
            {
                _logger.LogWarning("Tipo de evento não mapeado para projeção: {EventType}", envelope.EventType);
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IAccountReadRepository>();

            await projection.ProjectAsync(envelope, repository);
        }

        private void DeclareTopology()
        {
            _channel!.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Fanout,
                durable: true);

            _channel.QueueDeclare(
                queue: _settings.ReadModelQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _channel.QueueBind(
                queue: _settings.ReadModelQueueName,
                exchange: _settings.ExchangeName,
                routingKey: string.Empty);
        }

        public override void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            base.Dispose();
        }
    }
}
