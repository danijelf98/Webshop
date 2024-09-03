using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.Account_Models;
using Webshop.Shared.Models.Dto;

namespace Webshop.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IBuyerService buyerService;

        public AccountController(IAccountService accountService, IBuyerService buyerService)
        {
            this.accountService = accountService;
            this.buyerService = buyerService;
        }


        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegstrationBinding model)
        {
            await accountService.CreateUser(model, Roles.Buyer);
            return RedirectToAction("Categories", "Buyer");
        }

        [Authorize]
        public async Task<IActionResult> MyProfile()
        {
            var profile = await accountService.GetUserProfile<ApplicationUserUpdateBinding>(User);
            return View(profile);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MyProfile(ApplicationUserUpdateBinding model)
        {
            await accountService.UpdateUserProfle(model);
            return RedirectToAction("MyProfile");
        }
    }
}
