using System.Security.Claims;
using Webshop.Shared.Models.Binding.Account_Models;
using Webshop.Shared.Models.ViewModel.UserModels;

namespace Webshop.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUserViewModel?> CreateUser(RegstrationBinding model, string role);
        Task<T> GetUserAddress<T>(ClaimsPrincipal user);
        Task<ApplicationUserViewModel?> GetUserProfile(ClaimsPrincipal user);
        Task<T> GetUserProfile<T>(ClaimsPrincipal user);
        Task<ApplicationUserViewModel> UpdateUserProfle(ApplicationUserUpdateBinding model);
    }
}