﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.Base.OrderModels
{
    public abstract class OrderItemBase
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
