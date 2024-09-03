using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Services.Implementations;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.CompanyModels;
using Webshop.Shared.Models.Binding.ProductModels;
using Webshop.Shared.Models.Dto;

namespace Webshop.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly IBuyerService buyerService;
        private readonly IAdminService adminService;
        public AdminController(IProductService productService, IMapper mapper, IBuyerService buyerService, IAdminService adminService)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.buyerService = buyerService;
            this.adminService = adminService;
        }

        #region Product Category CRUD

        public async Task<IActionResult> Categories()
        {
            var categories = await productService.GetAllProductCategory();
            return View(categories);
        }

        public async Task<IActionResult> ProductDetails(long id)
        {
            var category = await productService.GetProductCategoryById(id);
            return View(category);
        }
        public async Task<IActionResult> AddProductCategory()
        {
            return View(); 
        }
        
        [HttpPost]
        public async Task<IActionResult> AddProductCategory(ProductCategoryBinding model)
        {
            var category = await productService.AddProductCategory(model);
            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> DeleteProductCategory(long id)
        {
            await productService.DeleteProductCategory(id);
            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> EditProductCategory(long id)
        {
            var model = await productService.GetProductCategoryById<ProductCategoryUpdateBinding>(id);
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> EditProductCategory(ProductCategoryUpdateBinding model)
        {
            await productService.EditProductCategory(model);
            return RedirectToAction("Categories");
        }

        #endregion

        #region Product Item CRUD

        public async Task<IActionResult> Items(long id)
        {
            var productCategory = await productService.GetProductCategoryById(id);
            return View(productCategory);
        }

        public async Task<IActionResult> ProductItemDetails(long id)
        {
            var item = await productService.GetProductItemById(id);
            return View(item);
        }
        public async Task<IActionResult> AddProductItem(long categoryId)
        {
            var model = new ProductItemBinding
            {
                ProductCategoryId = categoryId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductItem(ProductItemBinding model)
        {
            await productService.AddProductItem(model);
            return RedirectToAction("ProductDetails", new { Id = model.ProductCategoryId });
        }

        public async Task<IActionResult> DeleteProductItem(long id)
        {
            var response = await productService.DeleteProductItem(id);
            return RedirectToAction("ProductDetails", new { Id = response.ProductCategoryId });
        }

        #endregion

        #region Orders

        public async Task<IActionResult> Orders()
        {
            var orders = await buyerService.GetOrders(User);
            return View(orders);
        }
        public async Task<IActionResult> CancelOrder(long id)
        {
            await buyerService.CancelOrder(id);
            return RedirectToAction("Orders");
        }
        public async Task<IActionResult> Order(long id)
        {
            var order = await buyerService.GetOrder(id, User);
            return View(order);
        }

        #endregion

        public async Task<IActionResult> Company()
        {
            var company = await adminService.GetCompany<CompanyUpdateBinding>();
            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> Company(CompanyUpdateBinding model)
        {
            await adminService.UpdateCompay(model);
            return RedirectToAction("Company");
        }
    }




}

