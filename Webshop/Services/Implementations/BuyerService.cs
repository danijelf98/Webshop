using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webshop.Data;
using Webshop.Models.Dbo.OrderModels;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Models.Dbo.UserModel;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.OrderModels;
using Webshop.Shared.Models.Dto;
using Webshop.Shared.Models.ViewModel.OrderModels;

namespace Webshop.Services.Implementations
{
    public class BuyerService : IBuyerService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public BuyerService(IMapper mapper, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.db = db;
            this.userManager = userManager;
        }

        #region Order CRUD

        public async Task<List<OrderViewModel>> GetOrders()
        {
            var dbos = await db.Orders
                .Include(y => y.BuyerFeedbacks)
                .Include(y => y.Buyer)
                .Include(y => y.OrderItems)
                .Include(y => y.OrderAddress)
                .Where(y => y.Valid)
                .ToListAsync();

            foreach (var order in dbos)
            {
                order.BuyerFeedbacks = order.BuyerFeedbacks.Where(y => y.Valid).ToList();
            }

            return dbos.Select(y => mapper.Map<OrderViewModel>(y)).ToList();
        }
        public async Task<List<OrderViewModel>> GetOrders(ApplicationUser buyer)
        {
            var dbos = await db.Orders
                .Include(y => y.Buyer)
                .Include(y => y.OrderItems)
                .Include(y => y.OrderAddress)
                .Include(y => y.BuyerFeedbacks)
                .Where(y => y.Valid && y.BuyerId == buyer.Id)
                .ToListAsync();

            foreach (var order in dbos)
            {
                order.BuyerFeedbacks = order.BuyerFeedbacks.Where(y => y.Valid).ToList();
            }

            return dbos.Select(y => mapper.Map<OrderViewModel>(y)).ToList();
        }
        public async Task<List<OrderViewModel>> GetOrders(ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            var role = await userManager.GetRolesAsync(applicationUser);

            switch (role[0])
            {
                case Roles.Admin:
                    return await GetOrders();
                case Roles.Buyer:
                    return await GetOrders(applicationUser);
                default:
                    throw new NotImplementedException($"{role[0]} isn't implemented in get orders!");
            }
        }
        public async Task<OrderViewModel> GetOrder(long id)
        {
            var dbo = await db.Orders
                .Include(y => y.Buyer)
                .Include(y => y.OrderItems)
                .Include(y => y.OrderAddress)
                .FirstOrDefaultAsync(y => y.Id == id);

            return mapper.Map<OrderViewModel>(dbo);
        }

        /// <summary>
        /// Get order by role and order id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<OrderViewModel> GetOrder(long id, ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            var role = await userManager.GetRolesAsync(applicationUser);

            switch (role[0])
            {
                case Roles.Admin:
                    return await GetOrder(id);
                case Roles.Buyer:
                    return await GetOrder(id, applicationUser);
                default:
                    throw new NotImplementedException($"{role[0]} isn't implemented in get orders!");
            }
        }
        /// <summary>
        /// Get order by app user and order id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="buyer"></param>
        /// <returns></returns>
        public async Task<OrderViewModel> GetOrder(long id, ApplicationUser buyer)
        {
            var dbo = await db.Orders
                .Include(y => y.Buyer)
                .Include(y => y.OrderItems)
                .Include(y => y.BuyerFeedbacks)
                .Include(y => y.OrderAddress)
                .FirstOrDefaultAsync(y => y.Id == id && y.BuyerId == buyer.Id);

            dbo.BuyerFeedbacks = dbo.BuyerFeedbacks.Where(y => y.Valid).ToList();

            return mapper.Map<OrderViewModel>(dbo);
        }
        public async Task<OrderViewModel> AddOrder(OrderBinding model, ApplicationUser buyer)
        {
            var dbo = mapper.Map<Order>(model);
            var productItems = db.ProductItems
                .Where(y => model.OrderItems.Select(y => y.ProductItemId).Contains(y.Id)).ToList();


            foreach (var product in dbo.OrderItems)
            {
                var target = productItems.FirstOrDefault(y => product.ProductItemId == y.Id);
                if (target != null)
                {
                    target.Quantity -= product.Quantity;
                    product.Price = target.Price;
                }
            }

            dbo.OrderStatus = OrderStatus.Pending;
            dbo.Buyer = buyer;
            dbo.CalculateTotal();

            db.Orders.Add(dbo);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or print it to the console
                Console.WriteLine("DbUpdateException occurred: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }

                throw;
            }


            return mapper.Map<OrderViewModel>(dbo);
        }
        public async Task<OrderViewModel> AddOrder(OrderBinding model, ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            return await AddOrder(model, applicationUser);
        }

        public async Task<OrderViewModel> EditOrder(OrderUpdateBinding model)
        {
            var dbo = await db.Orders
                .Include(y => y.OrderItems)
                .Include(y => y.OrderAddress)
                .FirstOrDefaultAsync(y => y.Id == model.Id);
            mapper.Map(model, dbo);
            await db.SaveChangesAsync();

            return mapper.Map<OrderViewModel>(dbo);
        }

        /// <summary>
        /// Regulate Order Status
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderStatus"></param>
        /// <returns></returns>
        public async Task<OrderViewModel> RegulateOrderStatus(long orderId, OrderStatus orderStatus)
        {
            switch (orderStatus)
            {

                case OrderStatus.Canceled:
                    return await CancelOrder(orderId);

                default:
                    var dbo = await db.Orders.FindAsync(orderId);
                    dbo.OrderStatus = orderStatus;
                    return mapper.Map<OrderViewModel>(dbo);
            }
        }
        public async Task<OrderViewModel> CancelOrder(long id)
        {
            var dbo = await db.Orders
                .Include(y => y.OrderItems)
                .ThenInclude(y => y.ProductItem)
                .FirstOrDefaultAsync(y => y.Id == id);

            var productItems = db.ProductItems
                .Where(y => dbo.OrderItems.Select(y => y.ProductItemId).Contains(y.Id)).ToList();


            foreach (var product in dbo.OrderItems)
            {
                var target = productItems.FirstOrDefault(y => product.ProductItemId == y.Id);
                if (target != null)
                {
                    target.Quantity += product.Quantity;
                }
            }
            dbo.OrderStatus = OrderStatus.Canceled;
            await db.SaveChangesAsync();
            return mapper.Map<OrderViewModel>(dbo);
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderViewModel> DeleteOrder(long id)
        {
            await CancelOrder(id);

            var dbo = await db.Orders.FirstOrDefaultAsync(y => y.Id == id);

            dbo.Valid = false;
            await db.SaveChangesAsync();
            return mapper.Map<OrderViewModel>(dbo);
        }

        public async Task<List<BuyerFeedbackViewModel>> GetBuyerFeedbacks(long orderIds)
        {
            var dbos = db.BuyerFeedbacks
                .Include(y => y.Order)
                .Where(y => y.OrderId == orderIds && y.Valid);

            return dbos.Select(y => mapper.Map<BuyerFeedbackViewModel>(y)).ToList();
        }

        /// <summary>
        /// Adds Buyer Feedback
        /// </summary>
        /// <param name="model"></param>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<BuyerFeedbackViewModel> AddBuyerFeedback(BuyerFeedbackBinding model, ApplicationUser applicationUser)
        {
            var order = db.Orders.FirstOrDefaultAsync(y => y.Id == model.OrderId && y.BuyerId == applicationUser.Id);
            if (order == null)
            {
                throw new Exception("Buyer isn't Valid!");
            }

            var dbo = mapper.Map<BuyerFeedback>(order);
            db.BuyerFeedbacks.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<BuyerFeedbackViewModel>(dbo);
        }

        public async Task<BuyerFeedbackViewModel> AddBuyerFeedback(BuyerFeedbackBinding model)
        {

            var dbo = mapper.Map<BuyerFeedback>(model);
            db.BuyerFeedbacks.Add(dbo);
            db.SaveChanges();
            return mapper.Map<BuyerFeedbackViewModel>(dbo);


        }
        public async Task<BuyerFeedbackViewModel> DeleteBuyerFeedback(long id)
        {
            var dbo = await db.BuyerFeedbacks.FindAsync(id);
            dbo.Valid = false;
            await db.SaveChangesAsync();
            return mapper.Map<BuyerFeedbackViewModel>(dbo);
        }


        #endregion

    }
}
