using Catalog.Application.DTOs.Creation;
using Catalog.Application.DTOs.Filtering;
using Catalog.Application.DTOs.Pagination;
using Catalog.Application.DTOs.ReadOnly;
using Catalog.Application.DTOs.Sorting;
using Catalog.Domain.Entities;
using Catalog.Domain.Enums;

namespace Catalog.Application.Interfaces
{
    public interface IProductsService
    {
        Task<PaginatedResponse<ProductDto>> GetAsync(PaginationRequest pagination, ProductSortRequest? sort);
        Task<PaginatedResponse<ProductDto>> GetAsync(ProductFilter filter, PaginationRequest pagination, ProductSortRequest? sort);
        Task<PaginatedResponse<ProductDto>> GetByTypeAsync(ProductType type, PaginationRequest pagination, ProductSortRequest? sort);
        Task<ProductDto?> GetAsync(string id);
        Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken ct = default);
        Task UpdateAsync(string id, CreateProductRequest updatedProduct);
        Task RemoveAsync(string id);
        Task<List<Product>> GetAllByIdAsync(List<string> ids);
    }
}
