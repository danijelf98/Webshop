using System.ComponentModel.DataAnnotations;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.OrderModels;

namespace Webshop.Models.Dbo.OrderModels
{
    public class BuyerFeedback : BuyerFeedbackBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }

        public Order? Order { get; set; }
        public long? OrderId { get; set; } 
    }
}
