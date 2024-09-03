using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Webshop.Data;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.ProductModels;
using Webshop.Shared.Models.Dto;
using Webshop.Shared.Models.ViewModel.ProductModels;

namespace Webshop.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext db;
        private AppSettings appSettings;

        public ProductService(IMapper mapper, ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            this.mapper = mapper;
            this.db = db;
            this.appSettings = appSettings.Value;
        }


        #region Product Category CRUD

        /// <summary>
        /// Gets All Product Category
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductCategoryViewModel>> GetAllProductCategory(bool? valid = true)
        {
            var dbo = await db.ProductCategories
                .Include(y => y.ProductItems)
                .Where(y => y.Valid == valid)
                .ToListAsync();

            return dbo.Select(y => mapper.Map<ProductCategoryViewModel>(y)).ToList();
        }

        /// <summary>
        /// Gets all Product Category by using Pagination
        /// </summary>
        /// <param name="page"></param>
        /// <param name="searchTerm"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<ProductCategoryPaginationViewModel> GetAllProductCategory(int page, string? searchTerm = null, int? offset = null)
        {

            if (!offset.HasValue || offset.Value > 150)
            {
                offset = appSettings.PaginationOffset;
            }

            var baseQuery = db.ProductCategories
                .Include(y => y.ProductItems)
                .Where(y => y.Valid);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                baseQuery = baseQuery.Where(s => EF.Functions.Like(s.Name, $"%{searchTerm}%") || EF.Functions.Like(s.Description, $"%{searchTerm}%"));
            }
            var totalRecords = await baseQuery.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / offset.Value);
            var basePQuery = await baseQuery
                .Skip((page - 1) * offset.Value)
                .Take(offset.Value)
                .ToListAsync();

            var productCategory = basePQuery.OrderByDescending(y => y.Created).ToList();
            var response = new ProductCategoryPaginationViewModel
            {
                ProductCategories = basePQuery.Select(y => mapper.Map<ProductCategoryViewModel>(y)).ToList(),
                Rows = totalPages,
                TotalRecords = totalRecords,
            };

            return response;
        }


        /// <summary>
        /// Gets Product Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductCategoryViewModel> GetProductCategoryById(long id)
        {
            var dbo = await db.ProductCategories
                .Include(y => y.ProductItems)
                .FirstOrDefaultAsync(y => y.Id == id);

            dbo.ProductItems = dbo.ProductItems.Where(item => item.Valid).ToList();
            return mapper.Map<ProductCategoryViewModel>(dbo);
        }

        /// <summary>
        /// Gets Product Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetProductCategoryById<T>(long id)
        {
            var dbo = await db.ProductCategories
                .Include(y => y.ProductItems)
                .FirstOrDefaultAsync(y => y.Id == id);

            dbo.ProductItems = dbo.ProductItems.Where(item => item.Valid).ToList();
            return mapper.Map<T>(dbo);
        }

        /// <summary>
        /// Add Product Category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProductCategoryViewModel> AddProductCategory(ProductCategoryBinding model)
        {
            var company = await db.Companies.FirstOrDefaultAsync(y => y.Valid);
            var dbo = mapper.Map<ProductCategory>(model);
            dbo.Company = company;
            db.ProductCategories.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ProductCategoryViewModel>(dbo);
        }

        /// <summary>
        /// Edit Product Category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProductCategoryViewModel> EditProductCategory(ProductCategoryUpdateBinding model)
        {
            var dbo = await db.ProductCategories.FirstOrDefaultAsync(y => y.Id == model.Id);
            mapper.Map(model, dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ProductCategoryViewModel>(dbo);
        }

        /// <summary>
        /// Deletes Product Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductCategoryViewModel> DeleteProductCategory(long id)
        {
            var dbo = await db.ProductCategories.FirstOrDefaultAsync(y => y.Id == id);
            dbo.Valid = false;
            await db.SaveChangesAsync();
            return mapper.Map<ProductCategoryViewModel>(dbo);
        }

        #endregion

        #region Product Item CRUD

        /// <summary>
        /// Gets Product list by Ids
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ProductItemViewModel>> GetProductItems(List<long> id)
        {
            var dbo = await db.ProductItems.Where(y => id.Contains(y.Id)).ToListAsync();
            return dbo.Select(y => mapper.Map<ProductItemViewModel>(y)).ToList(); 
        }

        /// <summary>
        /// Gets All Product Items
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductItemViewModel>> GetAllProductItem()
        {
            var dbo = await db.ProductItems.ToListAsync();
            if (!dbo.Any())
            {
                return new List<ProductItemViewModel>();
            }
            return dbo.Select(y => mapper.Map<ProductItemViewModel>(y)).ToList();
        }

        /// <summary>
        /// Gets Product Item By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductItemViewModel> GetProductItemById(long id)
        {
            var dbo = await db.ProductItems
                .Include(y => y.ProductCategory)
                .Include(y => y.QuantityType)
                .FirstOrDefaultAsync(y => y.Id == id);
            return mapper.Map<ProductItemViewModel>(dbo);
        }

        /// <summary>
        /// Gets Product Item By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetProductItemById<T>(long id)
        {
            var dbo = await db.ProductItems
                .Include(y => y.ProductCategory)
                .Include(y => y.QuantityType)
                .FirstOrDefaultAsync(y => y.Id == id);
            return mapper.Map<T>(dbo);
        }

        /// <summary>
        /// Add Product Item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProductItemViewModel> AddProductItem(ProductItemBinding model)
        {
            var dbo = mapper.Map<ProductItem>(model);
            db.ProductItems.Add(dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ProductItemViewModel>(dbo);
        }

        /// <summary>
        /// Edit Product Item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ProductItemViewModel> EditProductItem(ProductItemUpdateBinding model)
        {
            var dbo = await db.ProductItems.FirstOrDefaultAsync(y => y.Id == model.Id);
            mapper.Map(model, dbo);
            await db.SaveChangesAsync();
            return mapper.Map<ProductItemViewModel>(dbo);
        }

        /// <summary>
        /// Deletes Product Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductItemViewModel> DeleteProductItem(long id)
        {
            var dbo = await db.ProductItems
                .Include(y => y.ProductCategory)
                .FirstOrDefaultAsync(y => y.Id == id);
            dbo.Valid = false;
            await db.SaveChangesAsync();
            return mapper.Map<ProductItemViewModel>(dbo);
        }

        #endregion


        /// <summary>
        /// Gets All Quantity Types
        /// </summary>
        /// <returns></returns>
        public async Task<List<QuantityTypeViewModel>> GetQuantityTypes()
        {
            var dbo = await db.QuantityTypes.Where(y => y.Valid).ToListAsync();
            if (!dbo.Any())
            {
                return new List<QuantityTypeViewModel>();
            }
            return dbo.Select(y => mapper.Map<QuantityTypeViewModel>(y)).ToList();
        }
    }
}
