using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webshop.Data;
using Webshop.Models.Dbo.UserModel;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.Account_Models;
using Webshop.Shared.Models.ViewModel.UserModels;

namespace Webshop.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private SignInManager<ApplicationUser> SignInManager;

        public AccountService(UserManager<ApplicationUser> userManager, ApplicationDbContext db, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.db = db;
            this.mapper = mapper;
            SignInManager = signInManager;
        }


        public async Task<ApplicationUserViewModel?> CreateUser(RegstrationBinding model, string role)
        {
            var find = await userManager.FindByNameAsync(model.Email);

            if (find != null)
            {
                return null;
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                RegistrationDate = DateTime.Now
            };

            user.EmailConfirmed = true;
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
                await userManager.UpdateAsync(user);
                await SignInManager.SignInAsync(user, false);
                return mapper.Map<ApplicationUserViewModel>(user);
            }

            return null;
        }

        /// <summary>
        /// Get User Address
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<T> GetUserAddress<T>(ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            var dboUser = db.Users
                .Include(x => x.Address)
                .FirstOrDefault(x => x.Id == applicationUser.Id);

            return mapper.Map<T>(dboUser.Address);
        }
        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ApplicationUserViewModel?> GetUserProfile(ClaimsPrincipal user)
        {
            var dbo = await db.Users
                .Include(y => y.Address)
                .FirstOrDefaultAsync(y => y.Id == userManager.GetUserId(user));

            return mapper.Map<ApplicationUserViewModel>(dbo);
        }

        /// <summary>
        /// Get User Profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<T> GetUserProfile<T>(ClaimsPrincipal user)
        {
            var dbo = await db.Users
                .Include(y => y.Address)
                .FirstOrDefaultAsync(y => y.Id == userManager.GetUserId(user));

            return mapper.Map<T>(dbo); 
        }

        /// <summary>
        /// Updates Application user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApplicationUserViewModel> UpdateUserProfle(ApplicationUserUpdateBinding model)
        {
            var dbo = await db.Users
                .Include(y => y.Address)
                .FirstOrDefaultAsync(y => y.Id == model.Id);
            
            mapper.Map(model, dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ApplicationUserViewModel>(dbo);
        }
    }
}
