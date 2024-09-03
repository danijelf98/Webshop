using System.ComponentModel.DataAnnotations;
using Webshop.Models.Dbo.CompanyModels;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.ProductModels;
using Webshop.Shared.Models.ViewModel.ProductModels;

namespace Webshop.Models.Dbo.ProductModels
{
    public class ProductCategory : ProductCategoryBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }

        public ICollection<ProductItem>? ProductItems { get; set; }
        public Company? Company { get; set; } 
        public long? CompanyId { get; set; }

    }
}
