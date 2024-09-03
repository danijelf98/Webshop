using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.Binding.ProductModels
{
    public abstract class QuantityTypeBase
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
