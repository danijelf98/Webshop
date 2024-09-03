using System.ComponentModel.DataAnnotations;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.ProductModels;

namespace Webshop.Models.Dbo.ProductModels
{
    public class ProductItem : ProductItemBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }

        public QuantityType? QuantityType { get; set; }
        public long? QuantityTypeId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public long? ProductCategoryId { get; set; }
    }
}
