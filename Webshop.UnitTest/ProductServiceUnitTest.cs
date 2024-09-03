using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Services.Interfaces;
using Webshop.Shared.Models.Binding.ProductModels;

namespace Webshop.UnitTest
{
    public class ProductServiceUnitTest : WebshopSetup
    {
        private readonly IProductService productService;

        public ProductServiceUnitTest()
        {
            this.productService = GetProductService();
        }

        #region Product Item Tests

        [Fact]
        public async void GetAllProductItems_GetsProductItemsFromDb_ValidatesIfListIsNotEmpty()
        {
            var response = await productService.GetAllProductItem();
            Assert.NotEmpty(response);
        }
        [Fact]
        public async void GetProductItemById_GetsProductItemByIdFromDb_ValidatesIfItemIsNotNull()
        {
            var response = await productService.GetProductItemById(1);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task AddProductItem_AddsProductItemToDatabase_ReturnsViewModel()
        {
            var response = await productService.AddProductItem(new ProductItemBinding
            {
                Description = TestString,
                Name = TestString,
                Price = 1233,
                ProductCategoryId = ProductCategories[1].Id,
                Quantity = 10
            });

            Assert.NotNull(response);
        }

        [Fact]
        public async Task UpdateProductItem_UpdatesProductItemInDatabase_ValidatesIfDeletedItemIsNull()
        {
            var response = await productService.EditProductItem(new ProductItemUpdateBinding
            {
                Id = 1,
                Description = TestString,
                Name = TestString,
            });

            Assert.NotNull(response);
            Assert.Equal(TestString, response.Name);
            Assert.Equal(TestString, response.Description);
        }

        [Fact]
        public async Task DeleteProductItem_DeletesProductItemToDatabase_valdiatesIfDeletedItemIsNull()
        {
            var addedItem = await productService.AddProductItem(new ProductItemBinding
            {
                Description = TestString + "x",
                Name = TestString,
                Price = 1233,
                ProductCategoryId = ProductCategories[12].Id,
                Quantity = 10
            });
            Assert.NotNull(addedItem);

            await productService.DeleteProductItem(addedItem.Id);
            var productCategory = await productService.GetProductCategoryById(ProductCategories[12].Id);
            var productItem = productCategory.ProductItems.FirstOrDefault(y => y.Id == addedItem.Id);
            Assert.Null(productItem);
        }

        

        #endregion

        #region Product Category Tests

        [Fact]
        public async Task GetAllProductCategories_GetsAllProductCategoriesFromDb_ValidatesIfListIsNotEmpty()
        {
            var response = await productService.GetAllProductCategory();
            Assert.NotEmpty(response);
        }

        [Fact]
        public async Task GetProductCategoryById_GetsProductCategoryByIdFromDb_ValidatesIfCategoryIsNotNull()
        {
            var response = await productService.GetProductCategoryById(1);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task AddProductCategory_AddsProductCategoryToDatabase_ReturnsViewModel()
        {
            var response = await productService.AddProductCategory(new ProductCategoryBinding
            {
                Description = TestString,
                Name = TestString,
            });

            Assert.NotNull(response);
            Assert.Equal(TestString, response.Name);
            Assert.Equal(TestString, response.Description);

            response = await productService.GetProductCategoryById(response.Id);
            Assert.NotNull(response);
        }
        [Fact]
        public async Task UpdateProductCategory_UpdatesProductCategoryInDatabase_ReturnsUpdatedViewModel()
        {
            var response = await productService.EditProductCategory(new ProductCategoryUpdateBinding
            {
                Id = ProductCategories[20].Id,
                Description = TestString,
                Name = TestString,
            });

            Assert.NotNull(response);
            Assert.Equal(TestString, response.Name);
            Assert.Equal(TestString, response.Description);
        }

        [Fact]
        public async Task DeleteProductCategory_DeletesProductCategoryInDatabase_ValidatesIfItemIsNull()
        {
            var productCategories = await productService.GetAllProductCategory();
            var firstProductCategory = await productService.GetProductCategoryById(1);

            Assert.Equal(50, productCategories.Count());
            Assert.NotNull(firstProductCategory);

            var deletedProductCategory = await productService.DeleteProductCategory(firstProductCategory.Id);
            productCategories = await productService.GetAllProductCategory();

            Assert.Null(productCategories.FirstOrDefault(y => y.Id == deletedProductCategory.Id));
            Assert.Equal(49, productCategories.Count());
        }

        #endregion
    }
}
