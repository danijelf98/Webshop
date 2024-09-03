using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.Common;
using Webshop.Shared.Models.Binding.OrderModels;
using Webshop.Shared.Models.Dto;

namespace Webshop.Controllers
{
    [Authorize(Roles = Roles.Buyer)]
    public class BuyerController : Controller
    {
        private readonly IBuyerService buyerService;
        private readonly IProductService productService;
        private readonly IAccountService accountService;
        private readonly ICommonService commonService;

        private static string OrderItemSessionKey = "OrderItems";
        public BuyerController(IBuyerService buyerService, IProductService productService, IAccountService accountService, ICommonService commonService)
        {
            this.buyerService = buyerService;
            this.productService = productService;
            this.accountService = accountService;
            this.commonService = commonService;
        }
        public async Task<IActionResult> MyOrders()
        {
            var orders = await buyerService.GetOrders(User);
            return View(orders);
        }
        public async Task<IActionResult> MyOrder(long id)
        {
            var order = await buyerService.GetOrder(id, User);
            if (order == null)
            {
                return Forbid();
            }
            return View(order);
        }
        public async Task<IActionResult> Order()
        {
            var sessionOrderItems = HttpContext.Session.GetString(OrderItemSessionKey);
            List<OrderItemBinding> existingOrderItems = sessionOrderItems != null ?
                JsonSerializer.Deserialize<List<OrderItemBinding>>(sessionOrderItems) : new List<OrderItemBinding>();


            if (!existingOrderItems.Any())
            {
                var sessionFromDb = await commonService.GetSessionItem<List<OrderItemBinding>>(OrderItemSessionKey, User);
                if (sessionFromDb != null)
                {
                    existingOrderItems = sessionFromDb;
                    HttpContext.Session.SetString(OrderItemSessionKey, JsonSerializer.Serialize(existingOrderItems));
                }
            }

            var response = new OrderBinding
            {
                OrderItems = existingOrderItems,
                OrderAddress = await accountService.GetUserAddress<AddressBinding>(User)
            };

            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Order(OrderBinding model)
        {
            var order = await buyerService.AddOrder(model, User);
            HttpContext.Session.Remove(OrderItemSessionKey);
            await commonService.RemoveFromSession(OrderItemSessionKey, User);

            return RedirectToAction("Categories");
        }
        public async Task<IActionResult> Categories()
        {
            var categories = await productService.GetAllProductCategory();
            return View(categories);
        }
        public async Task<IActionResult> ProductDetails(long id)
        {
            var category = await productService.GetProductCategoryById(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> AddToOrderItem([FromBody]List<OrderItemBinding> orderItems)
        {
            var sessionOrderItems = HttpContext.Session.GetString(OrderItemSessionKey);

            List<OrderItemBinding> existingOrderItems = sessionOrderItems != null ? 
                JsonSerializer.Deserialize<List<OrderItemBinding>>(sessionOrderItems) : new List<OrderItemBinding>();

            foreach (var orderItem in orderItems)
            {
                var existingOrderItem = existingOrderItems.FirstOrDefault(item => item.ProductItemId == orderItem.ProductItemId);
               
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity += orderItem.Quantity;
                }
                else
                {
                    existingOrderItems.Add(orderItem);
                }

            }

            await commonService.AddSessionItem(OrderItemSessionKey, existingOrderItems, User);
            HttpContext.Session.SetString(OrderItemSessionKey, JsonSerializer.Serialize(existingOrderItems));
            return Json(existingOrderItems);
        }
        public async Task<IActionResult> CancelOrder(long id)
        {
            await buyerService.CancelOrder(id);
            return RedirectToAction("MyOrders");
        }
        public async Task<IActionResult> AddBuyerFeedback(long orderId)
        {
            var order = new BuyerFeedbackBinding
            {
                OrderId = orderId
            };
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> AddBuyerFeedback(BuyerFeedbackBinding model)
        {
            await buyerService.AddBuyerFeedback(model);
            return RedirectToAction("MyOrders");
        }

    }
}
