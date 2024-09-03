﻿namespace Webshop.Services.Interfaces
{
    public interface IIdentitySetup
    {
        Task CreatePlatformAdminAsync();
        Task CreateRoleAsync(string role);
    }
}