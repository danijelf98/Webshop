using Webshop.Shared.Models.Binding.ProductModels;
using Webshop.Shared.Models.ViewModel.ProductModels;

namespace Webshop.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductCategoryViewModel> AddProductCategory(ProductCategoryBinding model);
        Task<ProductItemViewModel> AddProductItem(ProductItemBinding model);
        Task<ProductCategoryViewModel> DeleteProductCategory(long id);
        Task<ProductItemViewModel> DeleteProductItem(long id);
        Task<ProductCategoryViewModel> EditProductCategory(ProductCategoryUpdateBinding model);
        Task<ProductItemViewModel> EditProductItem(ProductItemUpdateBinding model);
        Task<List<ProductCategoryViewModel>> GetAllProductCategory(bool? valid = true);
        Task<ProductCategoryPaginationViewModel> GetAllProductCategory(int page, string? searchTerm = null, int? offset = null);
        Task<List<ProductItemViewModel>> GetAllProductItem();
        Task<ProductCategoryViewModel> GetProductCategoryById(long id);
        Task<T> GetProductCategoryById<T>(long id);
        Task<ProductItemViewModel> GetProductItemById(long id);
        Task<T> GetProductItemById<T>(long id);
        Task<List<ProductItemViewModel>> GetProductItems(List<long> id);
        Task<List<QuantityTypeViewModel>> GetQuantityTypes();
    }
}