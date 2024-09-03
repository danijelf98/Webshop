using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.OrderModels;

namespace Webshop.Shared.Models.ViewModel.OrderModels
{
    public class BuyerFeedbackViewModel : BuyerFeedbackBase
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
    }
}
