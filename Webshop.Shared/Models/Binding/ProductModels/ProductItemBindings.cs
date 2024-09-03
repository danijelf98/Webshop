using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.ProductModels;

namespace Webshop.Shared.Models.Binding.ProductModels
{
    public class ProductItemBinding : ProductItemBase
    {
        public long? ProductCategoryId { get; set; }
        public long? QuantityTypeId { get; set; }
    }
    public class ProductItemUpdateBinding : ProductItemBase
    {
        public long Id { get; set; }
        [Display(Name = "Measuring Unit")]
        public long? QuantityTypeId { get; set; }
    }
}
