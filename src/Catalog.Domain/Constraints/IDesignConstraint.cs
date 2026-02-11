using Catalog.Domain.Enums;

namespace Catalog.Domain.Constraints
{
    public interface IDesignConstraint
    {
        string Id { get; }
        ProductType DesignedFor { get; }
    }
}
