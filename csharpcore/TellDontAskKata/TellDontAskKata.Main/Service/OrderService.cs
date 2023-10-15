using System;
using System.Collections.Generic;
using System.Text;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Dtos;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Repository;

namespace TellDontAskKata.Main.Service
{

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IShipmentService shipmentService;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IShipmentService shipmentService)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.shipmentService = shipmentService;
        }

        public void ShipOrder(int orderId)
        {
            var order = orderRepository.GetById(orderId);

            switch (order.Status)
            {
                case OrderStatus.Rejected:
                case OrderStatus.Created:
                    throw new OrderCannotBeShippedException();
                case OrderStatus.Shipped:
                    throw new OrderCannotBeShippedTwiceException();
            }

            shipmentService.Ship(order);
            order.Status = OrderStatus.Shipped;
            orderRepository.Save(order);
        }

        public void CreateOrder(List<CreateOrderDto> orderDtos)
        {
            var createdOrder = new Order
            {
                Status = OrderStatus.Created,
                Currency = "EUR",
                Tax = 0m,
                Total = 0m
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
