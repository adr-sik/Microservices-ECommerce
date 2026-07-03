using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared.Messages.Contracts.Messages;
using System.Text.Json;


namespace Catalog.Infrastructure.Outbox
{
    public class OutboxRelayWorker : BackgroundService
    {
        private static readonly TimeSpan PollInterval = TimeSpan.FromMilliseconds(500);
        private const int MaxAttempts = 5;
        private const int BatchSize = 50;

        private readonly IMongoCollection<OutboxMessage> _collection;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxRelayWorker> _logger;

        private readonly Dictionary<string, Func<string, IPublishEndpoint, CancellationToken, Task>> _handlers = new()
        {
            [typeof(CatalogItemCreated).FullName!] = async (payload, pub, ct) =>
            {
                var msg = JsonSerializer.Deserialize<CatalogItemCreated>(payload)!;
                await pub.Publish(msg, ct);
            }
        };

        public OutboxRelayWorker(
            IServiceScopeFactory scopeFactory,
            OutboxCollection outbox,
            ILogger<OutboxRelayWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _collection = outbox.Collection;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OutboxRelayWorker started — polling every {Interval}ms", PollInterval.TotalMilliseconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RelayBatchAsync(stoppingToken);
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in relay loop");
                }

                await Task.Delay(PollInterval, stoppingToken);
            }
        }

        private async Task RelayBatchAsync(CancellationToken ct)
        {
            var pending = await _collection
                .Find(m => m.Status == OutboxStatus.Pending && m.Attempts < MaxAttempts)
                .SortBy(m => m.CreatedAt)
                .Limit(BatchSize)
                .ToListAsync(ct);

            if (pending.Count == 0) return;

            _logger.LogDebug("Relay found {Count} pending outbox message(s)", pending.Count);

            foreach (var message in pending)
            {
                await RelayMessageAsync(message, ct);
            }
        }

        private async Task RelayMessageAsync(OutboxMessage message, CancellationToken ct)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            var publish = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            try
            {
                if (!_handlers.TryGetValue(message.MessageType, out var handler))
                {
                    _logger.LogWarning(
                        "No handler registered for message type {MessageType} — marking Failed",
                        message.MessageType);
                    await MarkFailedAsync(message, "No handler registered", ct);
                    return;
                }

                await handler(message.Payload, publish, ct);

                await _collection.UpdateOneAsync(
                    m => m.Id == message.Id,
                    Builders<OutboxMessage>.Update
                        .Set(m => m.Status, OutboxStatus.Delivered)
                        .Set(m => m.DeliveredAt, DateTime.UtcNow)
                        .Inc(m => m.Attempts, 1),
                    cancellationToken: ct);

                _logger.LogInformation(
                    "Relayed outbox message {MessageId} ({MessageType})",
                    message.Id, message.MessageType);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Failed to relay outbox message {MessageId} (attempt {Attempt})",
                    message.Id, message.Attempts + 1);

                var newAttempts = message.Attempts + 1;
                var newStatus = newAttempts >= MaxAttempts ? OutboxStatus.Failed : OutboxStatus.Pending;

                await _collection.UpdateOneAsync(
                    m => m.Id == message.Id,
                    Builders<OutboxMessage>.Update
                        .Set(m => m.Status, newStatus)
                        .Set(m => m.LastError, ex.Message)
                        .Inc(m => m.Attempts, 1),
                    cancellationToken: ct);

                if (newStatus == OutboxStatus.Failed)
                    _logger.LogError(
                        "Outbox message {MessageId} exceeded max attempts — marked Failed. Investigate manually.",
                        message.Id);
            }
        }

        private async Task MarkFailedAsync(OutboxMessage message, string reason, CancellationToken ct)
        {
            await _collection.UpdateOneAsync(
                m => m.Id == message.Id,
                Builders<OutboxMessage>.Update
                    .Set(m => m.Status, OutboxStatus.Failed)
                    .Set(m => m.LastError, reason)
                    .Inc(m => m.Attempts, 1),
                cancellationToken: ct);
        }
    }
}
