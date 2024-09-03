using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.OrderModels;

namespace Webshop.Shared.Models.Binding.OrderModels
{
    public class OrderItemBinding
    {
        public decimal Quantity { get; set; }
        public long? ProductItemId { get; set; }
    }
    public class OrderItemUpdateBinding : OrderItemBase
    {
        public long Id { get; set; }
    }
}
