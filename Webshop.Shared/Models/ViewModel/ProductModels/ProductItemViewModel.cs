using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.ProductModels;

namespace Webshop.Shared.Models.ViewModel.ProductModels
{
    public class ProductItemViewModel : ProductItemBase
    {
        public long Id { get; set; }
        public long? ProductCategoryId { get; set; }
        public long? QuantityTypeId { get; set; }
    }
}
