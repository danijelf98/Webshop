using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.ProductModels;

namespace Webshop.Shared.Models.ViewModel.ProductModels
{
    public class ProductCategoryViewModel : ProductCategoryBase
    {
        public long Id { get; set; }
        public List<ProductItemViewModel>? ProductItems { get; set; }
    }
}
