using System.Security.Claims;
using Webshop.Shared.Models.Binding.CompanyModels;
using Webshop.Shared.Models.ViewModel.CompanyModels;

namespace Webshop.Services.Interfaces
{
    public interface IAdminService
    {
        Task<CompanyViewModel> GetCompany();
        Task<T> GetCompany<T>();
        Task<CompanyViewModel> UpdateCompay(CompanyUpdateBinding model);
    }
}