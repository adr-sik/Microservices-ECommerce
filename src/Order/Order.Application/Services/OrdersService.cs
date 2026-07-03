using MassTransit;
using Ordering.Application.DTOs;
using Ordering.Application.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Shared.Messages.Contracts.Saga;
using Shared.Messages.Contracts.Saga.Events;

namespace Ordering.Application.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ICatalogService _catalogService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _uow;

        public OrdersService(IOrdersRepository ordersRepository,
            ICatalogService catalogService,
            IPublishEndpoint publishEndpoint,
            IUnitOfWork uow)
        {
            _ordersRepository = ordersRepository;
            _catalogService = catalogService;
            _publishEndpoint = publishEndpoint;
            _uow = uow;
        }

        public async Task<Order> CreateAsync(CreateOrderRequest request, CancellationToken ct)
        {
            var productIds = request.Cart.Select(x => x.ProductId).ToList();
            var products = await _catalogService.GetProductsAsync(productIds);

            var orderItems = request.Cart.Select(c =>
            {
                var product = products.First(p => p.ProductId == c.ProductId);
                return OrderItem.Create(
                    product.ProductId,
                    product.ProductName,
                    c.Quantity,
                    (decimal)product.Price);
            }).ToList();

            var newOrder = Order.Create(request.UserId, orderItems);
            await _ordersRepository.CreateAsync(newOrder, ct);

            await _publishEndpoint.Publish<OrderSubmitted>(new
            {
                OrderId = newOrder.Id,
                Items = request.Cart.Select(i => new OrderItemContract
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            }, ct);

            await _uow.SaveChangesAsync(ct);

            return newOrder;
        }

        public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus, CancellationToken ct)
        {
            await _ordersRepository.UpdateStatusAsync(orderId, newStatus, ct);
        }
    }
}
