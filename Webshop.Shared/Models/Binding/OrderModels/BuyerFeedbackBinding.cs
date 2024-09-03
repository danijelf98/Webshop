using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.OrderModels;

namespace Webshop.Shared.Models.Binding.OrderModels
{
    public class BuyerFeedbackBinding : BuyerFeedbackBase
    {
        public long OrderId { get; set; }
    }
}
