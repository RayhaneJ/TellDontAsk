using System;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderShipmentUseCaseTest : BaseUseCastTest
    {
        [Fact]
        public void ShipApprovedOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Approved,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            orderService.ShipOrder(1);

            Assert.Equal(OrderStatus.Shipped, orderRepository.GetSavedOrder().Status);
            Assert.Same(initialOrder, shipmentService.GetShippedOrder());
        }

        [Fact]
        public void CreatedOrdersCannotBeShipped()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Created,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);
            Action actionToTest = () => orderService.ShipOrder(1);

            Assert.Throws<OrderCannotBeShippedException>(actionToTest);
            Assert.Null(orderRepository.GetSavedOrder());
            Assert.Null(shipmentService.GetShippedOrder());
        }

        [Fact]
        public void RejectedOrdersCannotBeShipped()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Rejected,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            Action actionToTest = () => orderService.ShipOrder(1);

            Assert.Throws<OrderCannotBeShippedException>(actionToTest);
            Assert.Null(orderRepository.GetSavedOrder());
            Assert.Null(shipmentService.GetShippedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeShippedAgain()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Shipped,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            Action actionToTest = () => orderService.ShipOrder(1);

            Assert.Throws<OrderCannotBeShippedTwiceException>(actionToTest);
            Assert.Null(orderRepository.GetSavedOrder());
            Assert.Null(shipmentService.GetShippedOrder());
        }


    }
}
