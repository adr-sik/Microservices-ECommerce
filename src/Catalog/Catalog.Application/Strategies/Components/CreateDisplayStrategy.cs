using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Components;
using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Application.Strategies.Components
{
    public class CreateDisplayStrategy : ICreateComponentStrategy<Display, CreateDisplayRequest>
    {
        public BaseComponent CreateComponent(CreateComponentRequest request)
        {
            if (request is not CreateDisplayRequest displayRequest)
                throw new ArgumentException("Invalid request type.");

            return new Display
            {
                DesignedFor = displayRequest.DesignedFor,
                ScreenSizeInches = displayRequest.ScreenSizeInches,
                Resolution = displayRequest.Resolution,
                RefreshRateHz = displayRequest.RefreshRateHz
            };
        }

        public BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate)
        {
            if (componentToUpdate is not Display display)
                throw new InvalidOperationException("Expected Display");

            if (request is not CreateDisplayRequest displayRequest)
                throw new ArgumentException("Invalid request type.");

            display.DesignedFor = displayRequest.DesignedFor;
            display.ScreenSizeInches = displayRequest.ScreenSizeInches;
            display.Resolution = displayRequest.Resolution;
            display.RefreshRateHz = displayRequest.RefreshRateHz;


            return display;
        }
    }
}
