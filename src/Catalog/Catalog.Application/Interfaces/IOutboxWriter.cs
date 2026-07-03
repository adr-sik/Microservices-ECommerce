namespace Catalog.Application.Interfaces
{
    public interface IOutboxWriter
    {
        Task WriteAsync<T>(T message, CancellationToken ct = default) where T : class;
    }
}
