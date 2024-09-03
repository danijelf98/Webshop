using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webshop.Data;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.CompanyModels;
using Webshop.Shared.Models.ViewModel.CompanyModels;

namespace Webshop.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public AdminService(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<CompanyViewModel> GetCompany()
        {
            var company = await db.Companies
                .Include(y => y.Address)
                .FirstOrDefaultAsync(y => y.Valid);
            return mapper.Map<CompanyViewModel>(company);
        }
        public async Task<T> GetCompany<T>()
        {
            var company = await db.Companies
                .Include(y => y.Address)
                .FirstOrDefaultAsync(y => y.Valid);
            return mapper.Map<T>(company);
        }

        /// <summary>
        /// Updates Company
        /// </summary>
        /// <param name="model"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        public async Task<CompanyViewModel> UpdateCompay(CompanyUpdateBinding model)
        {
            var dbo = await db.Companies
                .Include(y => y.Address)
                .FirstOrDefaultAsync(y => y.Id == model.Id);

            mapper.Map(model, dbo);
            await db.SaveChangesAsync();
            return mapper.Map<CompanyViewModel>(dbo);
        }
    }
}
