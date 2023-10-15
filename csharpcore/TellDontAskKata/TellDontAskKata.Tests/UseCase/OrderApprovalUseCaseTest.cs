using System;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.Service;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderApprovalUseCaseTest : BaseUseCastTest
    {
        [Fact]
        public void ApprovedExistingOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Created,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            orderService.ApproveOrder(1);

            var savedOrder = orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Approved, savedOrder.Status);
        }

        [Fact]
        public void RejectedExistingOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Created,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            orderService.RejectOrder(1);

            var savedOrder = orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Rejected, savedOrder.Status);
        }


        [Fact]
        public void CannotApproveRejectedOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Rejected,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            Action actionToTest = () => orderService.ApproveOrder(1);
      
            Assert.Throws<RejectedOrderCannotBeApprovedException>(actionToTest);
            Assert.Null(orderRepository.GetSavedOrder());
        }

        [Fact]
        public void CannotRejectApprovedOrder()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Approved,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            Action actionToTest = () => orderService.RejectOrder(1);
            
            Assert.Throws<ApprovedOrderCannotBeRejectedException>(actionToTest);
            Assert.Null(orderRepository.GetSavedOrder());
        }

        [Fact]
        public void ShippedOrdersCannotBeRejected()
        {
            var initialOrder = new Order
            {
                Status = OrderStatus.Shipped,
                Id = 1
            };
            orderRepository.AddOrder(initialOrder);

            Action actionToTest = () => orderService.RejectOrder(1);

            Assert.Throws<ShippedOrdersCannotBeChangedException>(actionToTest);
            Assert.Null(orderRepository.GetSavedOrder());
        }
    }
}
