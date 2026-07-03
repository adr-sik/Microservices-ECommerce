using Catalog.Application.Interfaces;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Transactions
{
    public class MongoDbUnitOfWork : IUnitOfWork, ITransactionIdProvider<IClientSessionHandle>
    {
        private readonly IMongoClient _mongoClient;
        public IClientSessionHandle? TransactionHandler { get; private set; }

        public MongoDbUnitOfWork(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public async Task StartTransactionAsync(CancellationToken ct = default)
        {
            if (TransactionHandler == null)
            {
                TransactionHandler = await _mongoClient.StartSessionAsync(cancellationToken: ct);
                TransactionHandler.StartTransaction();
            }
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (TransactionHandler == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await TransactionHandler.CommitTransactionAsync(cancellationToken: ct);
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (TransactionHandler == null)
                throw new InvalidOperationException("Transaction has not been started.");

            await TransactionHandler.AbortTransactionAsync(cancellationToken: ct);
        }

        public void Dispose() => TransactionHandler?.Dispose();
    }
}
