using Azure;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models.Dbo.OrderModels;
using Webshop.Models.Dbo.UserModel;
using Webshop.Services.Implementations;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.Common;
using Webshop.Shared.Models.Binding.OrderModels;
using Webshop.Shared.Models.Dto;

namespace Webshop.UnitTest
{
    public class BuyerServiceUnitTest : WebshopSetup
    {
        private readonly  IBuyerService buyerService;

        public BuyerServiceUnitTest()
        {
            this.buyerService = GetBuyerService();
        }

        [Fact]
        public async Task GetOrder_FetchesOrderFromDb_ValidatesIfIsNotNull()
        {
            var results = await buyerService.GetOrder(Orders[0].Id);
            Assert.NotNull(results);
        }
        [Fact]
        public async Task GetOrders_FetchesOrderFromDb_ValidatesIfItsNotEmpty()
        {
            var results = await buyerService.GetOrders(ApplicationUser);
            Assert.Single(results);
        }
        [Fact]
        public async Task AddOrder_AddsOrderInDb_ValidatesIfOrderIsNotNull()
        {
            var results = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            },ApplicationUser);
            
            Assert.NotNull(results);
            Assert.Equal(TestString, results.Message);

            results = await buyerService.GetOrder(results.Id);
            Assert.NotNull(results);
        }

        [Fact]
        public async Task EditOrder_EditsExistingOrder_ValidatesIfItsNotNull()
        {
            var order = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            }, ApplicationUser);

            var results = await buyerService.EditOrder
                (
                new OrderUpdateBinding()
                {
                    Id = order.Id,
                    Message = TestString + "2",
                    OrderAddress = new AddressUpdateBinding
                    {
                        City = "Zadar2",
                        Street = "Zadarska2",
                        Number = "113A",
                        Country = "CRO2",
                        Id = order.OrderAddress.Id
                    },
                });
            Assert.NotEqual(order.Message, results.Message);
            Assert.NotEqual(order.OrderAddress.Country, results.OrderAddress.Country);
        }

        [Fact]
        public async Task DeleteOrder_DeletesOrderInDb_ValidatesIfOrderIsDeleted()
        {
            var order = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            }, ApplicationUser);
            var previousOrders = await buyerService.GetOrders(ApplicationUser);
            int previousOrderCount = previousOrders.Count();

            await buyerService.DeleteOrder(order.Id);

            previousOrders = await buyerService.GetOrders(ApplicationUser);
            int previousOrdersCount = previousOrders.Count();

            await buyerService.DeleteOrder(order.Id);

            previousOrders = await buyerService.GetOrders(ApplicationUser);
            int newOrdersCount = previousOrders.Count();

            Asset.Equals(previousOrdersCount-1, newOrdersCount);
        }
        [Fact]
        public async Task CancelOrdere_CancelsOrderInDb_ValidatesIfOrderIsCanceled()
        {
            var order = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            }, ApplicationUser);
            var previousOrders = await buyerService.GetOrders(ApplicationUser);
            int previousOrderCount = previousOrders.Count();

            await buyerService.CancelOrder(order.Id);

            previousOrders = await buyerService.GetOrders(ApplicationUser);
            var previousOrder = previousOrders.FirstOrDefault(y => y.Id == order.Id);

            Asset.Equals(OrderStatus.Canceled, previousOrder.OrderStatus);
        }
        [Fact]
        public async Task RegulateOrderStatus_ChangesOrderStatus_ValidatesOrderStatus()
        {
            var order = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            }, ApplicationUser);

            await buyerService.RegulateOrderStatus(order.Id,OrderStatus.Processing);
            order = await buyerService.GetOrder(order.Id);

            Assert.Equal(OrderStatus.Processing, order.OrderStatus);
        }

        [Fact]
        public async Task AddBuyerFeedback_AddsBuyerFeedbackToOrder_ValidatesIfBuyerFeedbackIsNotNull()
        {
            var order = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            }, ApplicationUser);

            Assert.NotNull(order);

            var result = await buyerService.AddBuyerFeedback(new BuyerFeedbackBinding
            {
                Comment = TestString,
                OrderId = order.Id,
                Rating = 4
            });
            Assert.NotNull(result);
            Assert.Equal(4, result.Rating);
            Assert.Equal(order.Id, result.OrderId);

            var feedbacks = await buyerService.GetBuyerFeedbacks(order.Id);
            Assert.Single(feedbacks);
        }

        [Fact]
        public async Task DeleteBuyerFeedback_eletesBuyerFeedback_ValidatesIfBuyerFeedbackIsEmpty()
        {
            var order = await buyerService.AddOrder(new OrderBinding
            {
                Message = TestString,
                OrderAddress = new AddressBinding
                {
                    City = "Zadar",
                    Street = "Zadarska",
                    Number = "115A",
                    Country = "CRO"
                },
                OrderItems = new List<OrderItemBinding>
                    {
                        new OrderItemBinding
                        {
                            ProductItemId = ProductCategories[0].ProductItems.First().Id,
                            Quantity = 10,
                        }
                    }
            }, ApplicationUser);

            Assert.NotNull(order);

            var result = await buyerService.AddBuyerFeedback(new BuyerFeedbackBinding
            {
                Comment = TestString,
                OrderId = order.Id,
                Rating = 4
            });
           
            await buyerService.DeleteBuyerFeedback(result.Id);

            var feedbacks = await buyerService.GetBuyerFeedbacks(order.Id);
            Assert.Empty(feedbacks);
        }
    }
}
