using System;
using System.Collections.Generic;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Dtos;
using TellDontAskKata.Main.Exceptions;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderCreationUseCaseTest : BaseUseCastTest
    {
        [Fact]
        public void SellMultipleItems()
        {
            var saladeItem = new CreateOrderDto
            {
                ProductName = "salad",
                Quantity = 2
            };

            var tomatoItem = new CreateOrderDto
            {
                ProductName = "tomato",
                Quantity = 3
            };

            var createDtos = new List<CreateOrderDto> { saladeItem, tomatoItem };

            orderService.CreateOrder(createDtos);

            Order insertedOrder = orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Created, insertedOrder.Status);
            Assert.Equal(23.20m, insertedOrder.Total);
            Assert.Equal(2.13m, insertedOrder.Tax);
            Assert.Equal("EUR", insertedOrder.Currency);
            Assert.Equal(2, insertedOrder.Items.Count);
            Assert.Equal("salad", insertedOrder.Items[0].Product.Name);
            Assert.Equal(3.56m, insertedOrder.Items[0].Product.Price);
            Assert.Equal(2, insertedOrder.Items[0].Quantity);
            Assert.Equal(7.84m, insertedOrder.Items[0].TaxedAmount);
            Assert.Equal(0.72m, insertedOrder.Items[0].Tax);
            Assert.Equal("tomato", insertedOrder.Items[1].Product.Name);
            Assert.Equal(4.65m, insertedOrder.Items[1].Product.Price);
            Assert.Equal(3, insertedOrder.Items[1].Quantity);
            Assert.Equal(15.36m, insertedOrder.Items[1].TaxedAmount);
            Assert.Equal(1.41m, insertedOrder.Items[1].Tax);
        }

        [Fact]
        public void UnknownProduct()
        {
            Action actionToTest = () => orderService.CreateOrder(new List<CreateOrderDto> {
            new CreateOrderDto { ProductName ="unknown product"}
            });

            Assert.Throws<UnknownProductException>(actionToTest);
        }



    }
}
