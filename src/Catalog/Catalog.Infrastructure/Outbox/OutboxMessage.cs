using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        [BsonId]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        public string MessageType { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        [BsonRepresentation(BsonType.String)]
        public OutboxStatus Status { get; set; } = OutboxStatus.Pending;
        public DateTime? DeliveredAt { get; set; }
        public int Attempts { get; set; }
        public string? LastError { get; set; }
    }

    public enum OutboxStatus
    {
        Pending,
        Delivered,
        Failed
    }

    public class OutboxCollection
    {
        public const string Name = "outbox.messages";

        private readonly IMongoCollection<OutboxMessage> _collection;

        public OutboxCollection(IMongoDatabase database)
        {
            _collection = database.GetCollection<OutboxMessage>(Name);
            EnsureIndexes();
        }

        public IMongoCollection<OutboxMessage> Collection => _collection;

        private void EnsureIndexes()
        {
            _collection.Indexes.CreateOne(new CreateIndexModel<OutboxMessage>(
                Builders<OutboxMessage>.IndexKeys
                    .Ascending(m => m.Status)
                    .Ascending(m => m.CreatedAt),
                new CreateIndexOptions { Name = "status_createdat" }));
        }
    }
}
