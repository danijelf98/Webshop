using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.OrderModels;
using Webshop.Shared.Models.Binding.Common;

namespace Webshop.Shared.Models.Binding.OrderModels
{
    public class OrderBinding : OrderBase
    {
        public AddressBinding? OrderAddress { get; set; }
        public List<OrderItemBinding>? OrderItems { get; set; }
    }
    public class OrderUpdateBinding : OrderBase
    {
        public long Id { get; set; }
        public AddressUpdateBinding? OrderAddress { get; set; }
        public List<OrderItemUpdateBinding>? OrderItemIds { get; set; }
    }
}
