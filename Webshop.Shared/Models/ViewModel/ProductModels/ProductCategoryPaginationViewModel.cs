using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.ViewModel.ProductModels
{
    public class ProductCategoryPaginationViewModel
    {
        public int TotalRecords { get; set; }
        public int Rows { get; set; }
        public List<ProductCategoryViewModel> ProductCategories { get; set; }
    }
}
