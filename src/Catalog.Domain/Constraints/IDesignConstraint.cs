namespace Catalog.Domain.Constraints
{
    public interface IDesignConstraint
    {
        string Id { get; }
        Enums.ProductTypesEnum DesignedFor { get; }
    }
}
