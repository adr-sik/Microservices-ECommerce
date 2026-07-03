using Catalog.Application.DTOs.Sorting;
using Catalog.Domain.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Sorting
{
    public static class ProductSorterBuilder
    {
        public static SortDefinition<Product> BuildSort(ProductSortRequest sort)
        {
            var builder = Builders<Product>.Sort;

            Expression<Func<Product, object>> fieldSelector = sort.Column switch
            {
                ProductSortColumn.Price => p => p.Price,
                ProductSortColumn.Brand => p => p.Brand,
                ProductSortColumn.Model => p => p.Model,
                _ => p => p.Price
            };

            return sort.Direction == ProductSortDirection.Ascending
                ? builder.Ascending(fieldSelector)
                : builder.Descending(fieldSelector);
        }
    }
}
