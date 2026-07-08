using EWallet.Domain.Entities;
using System.Text.Json;

namespace EWallet.Domain.Helpers
{
    public static class EventDeserializer
    {
        private static readonly Dictionary<string, Type> _cache = new();

        public static DomainEvent Deserialize(EventRecord record)
        {
            var type = ResolveType(record.EventType)
                ?? throw new InvalidOperationException($"Tipo de evento desconhecido: {record.EventType}");

            return (DomainEvent?)JsonSerializer.Deserialize(record.EventData, type,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException($"Falha ao deserializar evento: {record.EventType}");
        }

        private static Type? ResolveType(string eventTypeName)
        {
            if (_cache.TryGetValue(eventTypeName, out var cached))
                return cached;

            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == eventTypeName
                    && typeof(DomainEvent).IsAssignableFrom(t)
                    && !t.IsAbstract);

            if (type is not null)
                _cache[eventTypeName] = type;

            return type;
        }
    }
}
