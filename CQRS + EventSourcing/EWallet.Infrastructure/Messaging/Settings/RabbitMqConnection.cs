using RabbitMQ.Client;

namespace EWallet.Infrastructure.Messaging.Settings
{
    public class RabbitMqConnection : IDisposable
    {
        private readonly RabbitMqSettings _settings;
        private IConnection? _connection;
        private readonly object _lock = new();
        private bool _disposed;

        public RabbitMqConnection(RabbitMqSettings settings)
        {
            _settings = settings;
        }

        public IConnection GetConnection()
        {
            if (_connection is { IsOpen: true })
                return _connection;

            lock (_lock)
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                var factory = new ConnectionFactory
                {
                    HostName = _settings.HostName,
                    Port = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password,
                    DispatchConsumersAsync = true
                };

                _connection = factory.CreateConnection();
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
