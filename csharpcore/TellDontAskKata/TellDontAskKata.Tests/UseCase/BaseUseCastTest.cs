using System;
using System.Collections.Generic;
using System.Text;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.Service;
using TellDontAskKata.Tests.Doubles;

namespace TellDontAskKata.Tests.UseCase
{
    public class BaseUseCastTest
    {
        protected readonly TestOrderRepository orderRepository;
        protected readonly IProductRepository productRepository;
        protected readonly TestShipmentService shipmentService;
        protected readonly IOrderService orderService;

        public BaseUseCastTest()
        {
            orderRepository = new TestOrderRepository();

            var food = new Category
            {
                Name = "food",
                TaxPercentage = 10m
            };
            var products = new List<Product>
            {
                new Product()
                {
                    Name = "salad",
                    Price = 3.56m,
                    Category = food,
                   
                },
                new Product()
                {
                    Name = "tomato",
                    Price=4.65m,
                    Category=food,
                }
            };
            productRepository = new InMemoryProductCatalog(products);
            shipmentService = new TestShipmentService();
            orderService = new OrderService(orderRepository, productRepository, shipmentService);   
        }


    }
}
