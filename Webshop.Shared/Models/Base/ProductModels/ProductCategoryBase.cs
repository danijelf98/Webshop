using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.Base.ProductModels
{
    public abstract class ProductCategoryBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
