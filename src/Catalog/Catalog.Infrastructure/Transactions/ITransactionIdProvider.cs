namespace Catalog.Infrastructure.Transactions
{
    public interface ITransactionIdProvider<out T>
    {
        T TransactionHandler { get; }
    }
}
