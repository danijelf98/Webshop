using System.Security.Claims;
using Webshop.Models.Dbo.UserModel;
using Webshop.Shared.Models.Binding.OrderModels;
using Webshop.Shared.Models.Dto;
using Webshop.Shared.Models.ViewModel.OrderModels;

namespace Webshop.Services.Interfaces
{
    public interface IBuyerService
    {
        Task<OrderViewModel> AddOrder(OrderBinding model, ApplicationUser buyer);
        Task<OrderViewModel> AddOrder(OrderBinding model, ClaimsPrincipal user);
        Task<OrderViewModel> CancelOrder(long id);
        Task<OrderViewModel> EditOrder(OrderUpdateBinding model);
        Task<List<OrderViewModel>> GetOrders();
        Task<List<OrderViewModel>> GetOrders(ApplicationUser buyer);
        Task<List<OrderViewModel>> GetOrders(ClaimsPrincipal user);
        Task<OrderViewModel> GetOrder(long id);
        Task<OrderViewModel> GetOrder(long id, ClaimsPrincipal user);
        Task<OrderViewModel> GetOrder(long id, ApplicationUser buyer);
        Task<OrderViewModel> DeleteOrder(long id);
        Task<OrderViewModel> RegulateOrderStatus(long orderId, OrderStatus orderStatus);
        Task<BuyerFeedbackViewModel> AddBuyerFeedback(BuyerFeedbackBinding model, ApplicationUser applicationUser);
        Task<BuyerFeedbackViewModel> DeleteBuyerFeedback(long id);
        Task<List<BuyerFeedbackViewModel>> GetBuyerFeedbacks(long orderIds);
        Task<BuyerFeedbackViewModel> AddBuyerFeedback(BuyerFeedbackBinding model);
    }
}