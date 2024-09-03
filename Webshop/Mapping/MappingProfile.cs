using AutoMapper;
using Webshop.Models.Dbo.Common;
using Webshop.Models.Dbo.CompanyModels;
using Webshop.Models.Dbo.OrderModels;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Models.Dbo.UserModel;
using Webshop.Shared.Models.Binding.Account_Models;
using Webshop.Shared.Models.Binding.Common;
using Webshop.Shared.Models.Binding.CompanyModels;
using Webshop.Shared.Models.Binding.OrderModels;
using Webshop.Shared.Models.Binding.ProductModels;
using Webshop.Shared.Models.ViewModel.Common;
using Webshop.Shared.Models.ViewModel.CompanyModels;
using Webshop.Shared.Models.ViewModel.OrderModels;
using Webshop.Shared.Models.ViewModel.ProductModels;
using Webshop.Shared.Models.ViewModel.UserModels;

namespace Webshop.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCategoryBinding, ProductCategory>();
            CreateMap<ProductCategoryUpdateBinding, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<ProductCategory, ProductCategoryUpdateBinding>();
            CreateMap<ProductCategoryViewModel, ProductCategoryUpdateBinding>();

            CreateMap<ProductItemBinding, ProductItem>();
            CreateMap<ProductItemUpdateBinding, ProductItem>();
            CreateMap<ProductItem, ProductItemViewModel>();
            CreateMap<ProductItem, ProductItemUpdateBinding>();
            CreateMap<ProductItemViewModel, ProductItemUpdateBinding>();

            CreateMap<QuantityType, QuantityTypeViewModel>();

            CreateMap<AddressBinding, Address>();
            CreateMap<AddressUpdateBinding, Address>();
            CreateMap<Address, AddressUpdateBinding>();
            CreateMap<Address, AddressViewModel>();
            CreateMap<Address, AddressBinding>();

            CreateMap<ApplicationUser, ApplicationUserViewModel>();
            CreateMap<ApplicationUser, ApplicationUserUpdateBinding>();
            CreateMap<ApplicationUserUpdateBinding, ApplicationUser>();

            CreateMap<OrderBinding, Order>();
            CreateMap<OrderUpdateBinding, Order>();
            CreateMap<Order, OrderViewModel>();

            CreateMap<OrderItemBinding, OrderItem>();
            CreateMap<OrderItemUpdateBinding, OrderItem>();
            CreateMap<OrderItem, OrderItemViewModel>();

            CreateMap<BuyerFeedbackBinding, BuyerFeedback>();
            CreateMap<BuyerFeedback, BuyerFeedbackViewModel>();

            CreateMap<CompanyUpdateBinding, Company>();
            CreateMap<Company, CompanyUpdateBinding>();
            CreateMap<Company, CompanyViewModel>();
        }
    }
}
