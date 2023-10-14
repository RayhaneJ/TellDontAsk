using System;
using System.Collections.Generic;
using System.Text;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Repository;

namespace TellDontAskKata.Main.Service
{
    public class CreateOrderDto
    {
        public int Quantity { get; set; }
        public string ProductName { get; set; }
    }

    public class OrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
        }

        public void CreateOrder(List<CreateOrderDto> orderDtos)
        {
            var createdOrder = new Order
            {
                Status = OrderStatus.Created,
                Currency = "EUR",
            };

            orderDtos.ForEach(orderDto =>
            {
                OrderItem createdOrderItem = CreateOrderItem(orderDto);
                createdOrder.Items.Add(createdOrderItem);
            });

            orderRepository.Save(createdOrder);
        }

        private OrderItem CreateOrderItem(CreateOrderDto orderDto)
        {
            var product = productRepository.GetByName(orderDto.ProductName);

            if (product is null)
                throw new UnknownProductException();

            var createdOrderItem = new OrderItem
            {
                Product = product,
                Quantity = orderDto.Quantity
            };
            return createdOrderItem;
        }

        public void ApproveOrder(int orderId)
        {
            var order = orderRepository.GetById(orderId);

            if (order.Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (order.Status == OrderStatus.Rejected)
                throw new RejectedOrderCannotBeApprovedException();

            order.Status = OrderStatus.Approved;
            orderRepository.Save(order);
        }

        public void RejectOrder(int orderId)
        {
            var order = orderRepository.GetById(orderId);

            if (order.Status == OrderStatus.Shipped)
                throw new ShippedOrdersCannotBeChangedException();

            if (order.Status == OrderStatus.Approved)
                throw new ApprovedOrderCannotBeRejectedException();

            order.Status = OrderStatus.Rejected;
            orderRepository.Save(order);
        }
    }
}
