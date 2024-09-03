using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using Webshop.Data;
using Webshop.Models.Dbo.Common;
using Webshop.Models.Dbo.UserModel;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Dto;

namespace Webshop.Services.Implementations
{
    public class CommonService : ICommonService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private AppSettings appSettings;
        public CommonService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IOptions<AppSettings> appSettings)
        {
            this.db = db;
            this.userManager = userManager;
            this.appSettings = appSettings.Value;
        }
        public async Task DeactivateAllExpiredSessions()
        {

            var expiredSessions = await db.SessionItems
                 .Where(y => y.Expires < DateTime.Now)
                 .ToListAsync();

            if (!expiredSessions.Any())
            {
                return;
            }

            foreach (var session in expiredSessions)
            {
                db.SessionItems.Remove(session);
            }

            await db.SaveChangesAsync();
        }
        public async Task RemoveFromSession(string key, ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            var dbo = await db.SessionItems
                .Include(y => y.User)
                .FirstOrDefaultAsync(y => y.Key == key
                && y.UserId == applicationUser.Id
                && y.Expires > DateTime.Now.AddHours(appSettings.ExpireSessionInHours * -1));

            if (dbo != null)
            {
                db.SessionItems.Remove(dbo);
                await db.SaveChangesAsync();
            }
        }
        public async Task AddSessionItem(string key, object value, ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            await RemoveFromSession(key, user);

            var dbo = new SessionItem
            {
                Key = key,
                Value = JsonSerializer.Serialize(value),
                UserId = applicationUser.Id,
                Expires = DateTime.Now.AddHours(appSettings.ExpireSessionInHours)
            };

            db.SessionItems.Add(dbo);
            await db.SaveChangesAsync();

        }
        public async Task<T> GetSessionItem<T>(string key, ClaimsPrincipal user)
        {
            var applicationUser = await userManager.GetUserAsync(user);
            var dbo = await db.SessionItems
                .Include(y => y.User)
                .FirstOrDefaultAsync(y => y.Key == key && y.UserId == applicationUser.Id && y.Expires > DateTime.Now);


            if (dbo == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(dbo.Value);
        }
    }
}
