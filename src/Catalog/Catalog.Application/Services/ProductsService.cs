using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Pagination;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.DTOs.Sorting;
using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Mapster;
using Microsoft.Extensions.Logging;
using Shared.Messages.Contracts.Messages;

namespace Catalog.Application.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductFactory _productFactory;
        private readonly IUnitOfWork _uow;
        private readonly IOutboxWriter _outboxWriter;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IProductsRepository productsRepository,
            IProductFactory productFactory,
            IUnitOfWork uow,
            IOutboxWriter outboxWriter,
            ILogger<ProductsService> logger)
        {
            _productsRepository = productsRepository;
            _productFactory = productFactory;
            _uow = uow;
            _outboxWriter = outboxWriter;
            _logger = logger;
        }

        public async Task<PaginatedResponse<ProductDto>> GetAsync(
            PaginationRequest pagination, ProductSortRequest? sort)
        {
            var (products, totalCount) = await _productsRepository.GetAsync(pagination, sort);
            var items = products.Adapt<IReadOnlyList<ProductDto>>();

            return new PaginatedResponse<ProductDto>(
                items,
                pagination.Page,
                pagination.PageSize,
                totalCount
                );
        }

        public async Task<PaginatedResponse<ProductDto>> GetAsync(
            ProductFilter filter, PaginationRequest pagination, ProductSortRequest? sort)
        {
            var (products, totalCount) = await _productsRepository.GetAsync(filter, pagination, sort);
            var items = products.Adapt<IReadOnlyList<ProductDto>>();

            return new PaginatedResponse<ProductDto>(
                items,
                pagination.Page,
                pagination.PageSize,
                totalCount
                );
        }

        public async Task<PaginatedResponse<ProductDto>> GetByTypeAsync(
            ProductType type, PaginationRequest pagination, ProductSortRequest? sort)
        {
            var (products, totalCount) = await _productsRepository.GetByTypeAsync(type, pagination, sort);
            var items = products.Adapt<IReadOnlyList<ProductDto>>();

            return new PaginatedResponse<ProductDto>(
                items,
                pagination.Page,
                pagination.PageSize,
                totalCount
                );
        }

        public async Task<ProductDto?> GetAsync(string id)
        {
            var product = await _productsRepository.GetAsync(id);
            return TypeAdapter.Adapt<Product, ProductDto>(product);
        }

        public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken ct)
        {
            await _uow.StartTransactionAsync(ct);

            try
            {
                var newProduct = await _productFactory.BuildProductAsync(request);
                await _productsRepository.CreateAsync(newProduct, ct);
                var message = new CatalogItemCreated
                {
                    ItemId = newProduct.Id,
                    CreatedAt = DateTime.UtcNow
                };
                await _outboxWriter.WriteAsync(message, ct);
                await _uow.CommitTransactionAsync(ct);
                return TypeAdapter.Adapt<Product, ProductDto>(newProduct);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while creating a new product.");
                await _uow.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task UpdateAsync(string id, CreateProductRequest request)
        {
            var productToUpdate = await _productsRepository.GetAsync(id);
            var updatedProduct = await _productFactory.ReplaceProductAsync(request, productToUpdate);
            await _productsRepository.UpdateAsync(id, updatedProduct);
        }

        public async Task RemoveAsync(string id) =>
            await _productsRepository.RemoveAsync(id);

        public async Task<List<Product>> GetAllByIdAsync(List<string> ids)
        {
            return await _productsRepository.GetAllByIdAsync(ids);
        }
    }
}
