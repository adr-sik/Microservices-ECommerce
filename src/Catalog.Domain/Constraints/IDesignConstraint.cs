using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Constraints
{
    public interface IDesignConstraint
    {
        string Id { get; }
        Enums.ProductTypesEnum DesignedFor { get; }
    }
}
