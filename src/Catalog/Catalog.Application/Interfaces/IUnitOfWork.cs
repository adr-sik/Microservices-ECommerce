namespace Catalog.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task StartTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
