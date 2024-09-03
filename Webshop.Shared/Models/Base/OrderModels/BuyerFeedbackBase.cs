using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.Base.OrderModels
{
    public abstract class BuyerFeedbackBase
    {
        public string? Comment { get; set; }
        public int Rating { get; set; }
    }
}
