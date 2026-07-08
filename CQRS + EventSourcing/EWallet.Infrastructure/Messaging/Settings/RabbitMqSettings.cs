namespace EWallet.Infrastructure.Messaging.Settings
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string ExchangeName { get; set; } = "ewallet.events";
        public string EventStoreQueueName { get; set; } = "ewallet.eventstore";
        public string ReadModelQueueName { get; set; } = "ewallet.readmodel";
    }
}
