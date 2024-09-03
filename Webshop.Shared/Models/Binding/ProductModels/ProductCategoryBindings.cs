using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.ProductModels;

namespace Webshop.Shared.Models.Binding.ProductModels
{
    public class ProductCategoryBinding : ProductCategoryBase
    {
    }
    public class ProductCategoryUpdateBinding : ProductCategoryBase
    {
        public long Id { get; set; }
    }
}
