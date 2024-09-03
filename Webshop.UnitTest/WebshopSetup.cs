using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Data;
using Webshop.Mapping;
using Webshop.Models.Dbo.Common;
using Webshop.Models.Dbo.OrderModels;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Models.Dbo.UserModel;
using Webshop.Services.Implementations;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Dto;

namespace Webshop.UnitTest
{
    public abstract class WebshopSetup
    {
        protected IMapper mapper { get; private set; }
        protected ApplicationDbContext InMemoryDbContext;
        protected readonly Mock<IOptions<AppSettings>> AppSettings;
        protected static string TestString = "testing";
        protected readonly List<ProductCategory> ProductCategories;
        protected readonly List<Order> Orders;
        protected readonly ApplicationUser ApplicationUser;
        protected readonly Mock<UserManager<ApplicationUser>> UserManager;

        protected WebshopSetup()
        {
            SetupInMemoryContext();
     
            var userStoreMock = Mock.Of<IUserStore<ApplicationUser>>();
            UserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock, null, null, null, null, null, null, null, null);
            ApplicationUser = GetApplicationUser();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
            var config = new AppSettings()
            {
                PaginationOffset = 10
            };
            AppSettings = new Mock<IOptions<AppSettings>>();
            AppSettings.Setup(c => c.Value).Returns(config);
            ProductCategories = GenerateProductCategories(50);
            Orders = GetOrders(1);

        }
        protected ApplicationUser GetApplicationUser()
        {
            var applicationUser =  new ApplicationUser()
            {
                UserName = "Test",
                Email = "test@test.com",
                Address = new Address
                {
                    City = "Varazdin",
                    Country = "Croatia",
                    Street = "Zagrebacka",
                    Number = "10A"
                },
                LastName = "Test",
                FirstName = "Test",
                PhoneNumber = "123456789",
                EmailConfirmed = true,
            };

            InMemoryDbContext.Users.Add(applicationUser); 
            InMemoryDbContext.SaveChanges();
            return applicationUser;
        }
        protected List<Order> GetOrders(int number)
        {
            List<Order> orders = new List<Order>();

            for (int i = 0; i < number; i++)
            {
                var order = new Order
                {
                    Buyer = ApplicationUser,
                    BuyerId = ApplicationUser.Id,
                    OrderAddress = new Address
                    {
                        City = "Varazdin",
                        Country = "Croatia",
                        Street = "Zagrebacka",
                        Number = "10A"
                    },
                    Message = TestString,
                    OrderItems = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            ProductItem = ProductCategories[0].ProductItems.First(),
                            Quantity = 10,
                            Price = 200,
                        }
                    }

                };
                InMemoryDbContext.Orders.Add(order);
                InMemoryDbContext.SaveChanges();
                orders.Add(order);
            }
            return orders;
        }

        protected List<ProductCategory> GenerateProductCategories(int number)
        {

            List<ProductCategory> response = new List<ProductCategory>();
            Random random = new Random();

            for (int i = 0; i < number; i++)
            {

                if (i != 0)
                {
                    ProductCategory listItem = new ProductCategory
                    {
                        Description = $"{nameof(ProductCategory.Description)} {random.Next(1, 1000)}",
                        Name = $"{nameof(ProductCategory.Name)} {random.Next(1, 1000)}",
                    };
                    response.Add(listItem);
                }
                else
                {
                    ProductCategory listItem = new ProductCategory
                    {
                        Description = $"{nameof(ProductCategory.Description)} {random.Next(1, 1000)}",
                        Name = $"{TestString} {random.Next(1, 1000)}",
                        ProductItems = new List<ProductItem>()
                        {
                            new ProductItem
                            {
                                Description = TestString,
                                Quantity  = 10,
                                Price = 20,
                                Name = TestString
                            },
                            new ProductItem
                            {
                                Description = TestString,
                                Quantity  = 15,
                                Price = 200,
                                Name = TestString
                            }
                        }
                    };

                    response.Add(listItem);
                }


            }

            InMemoryDbContext.ProductCategories.AddRange(response);
            InMemoryDbContext.SaveChanges();

            return response;

        }

        protected IProductService GetProductService(ApplicationDbContext? db = null)
        {
            if (db != null)
            {
                return new ProductService(mapper, db, AppSettings.Object);
            }
            return new ProductService(mapper, InMemoryDbContext, AppSettings.Object);

        }
        protected IBuyerService GetBuyerService(ApplicationDbContext? db = null)
        {
            if (db != null)
            {
                return new BuyerService(mapper, db, UserManager.Object);
            }
            return new BuyerService(mapper, InMemoryDbContext, UserManager.Object);

        }


        private void SetupInMemoryContext()
        {
            var inMemoryOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                            .Options;
            InMemoryDbContext = new ApplicationDbContext(inMemoryOptions);
        }
    }
}
