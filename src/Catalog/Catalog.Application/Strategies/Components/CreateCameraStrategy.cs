using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Creation.Components;
using Catalog.Domain.Entities.ProductComponents;

namespace Catalog.Application.Strategies.Components
{
    public class CreateCameraStrategy : ICreateComponentStrategy<Camera, CreateCameraRequest>
    {
        public BaseComponent CreateComponent(CreateComponentRequest request)
        {
            if (request is not CreateCameraRequest cameraRequest)
                throw new ArgumentException("Invalid request type.");

            return new Camera
            {
                DesignedFor = cameraRequest.DesignedFor,
                Megapixels = cameraRequest.Megapixels,
            };
        }

        public BaseComponent ReplaceComponent(CreateComponentRequest request, BaseComponent componentToUpdate)
        {
            if (componentToUpdate is not Camera camera)
                throw new InvalidOperationException("Expected Camera");

            if (request is not CreateCameraRequest cameraRequest)
                throw new ArgumentException("Invalid request type.");

            camera.DesignedFor = cameraRequest.DesignedFor;
            camera.Megapixels = cameraRequest.Megapixels;

            return camera;
        }
    }
}
