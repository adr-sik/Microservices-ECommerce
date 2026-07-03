using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Transactions;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Outbox
{
    public class OutboxWriter : IOutboxWriter
    {
        private readonly IMongoCollection<OutboxMessage> _collection;
        private readonly ITransactionIdProvider<IClientSessionHandle?> _sessionProvider;

        public OutboxWriter(OutboxCollection outbox, ITransactionIdProvider<IClientSessionHandle> sessionProvider)
        {
            _collection = outbox.Collection;
            _sessionProvider = sessionProvider;
        }

        public async Task WriteAsync<T>(T message, CancellationToken ct = default) where T : class
        {
            var session = _sessionProvider.TransactionHandler
                ?? throw new InvalidOperationException("No active transaction.");

            var outboxMessage = new OutboxMessage
            {
                MessageType = typeof(T).FullName!,
                Payload = JsonSerializer.Serialize(message),
                Exchange = typeof(T).Name,
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };

            await _collection.InsertOneAsync(session, outboxMessage, cancellationToken: ct);
        }
    }
}
