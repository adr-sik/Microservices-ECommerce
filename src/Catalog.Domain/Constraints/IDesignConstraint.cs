namespace Catalog.Domain.Constraints
{
    public interface IDesignConstraint
    {
        string Id { get; }
        Enums.ProductTypes DesignedFor { get; }
    }
}
