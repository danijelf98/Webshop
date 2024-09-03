using System.ComponentModel.DataAnnotations;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.OrderModels;

namespace Webshop.Models.Dbo.OrderModels
{
    public class OrderItem : OrderItemBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }

        public ProductItem? ProductItem { get; set; }
        public long? ProductItemId { get; set; }

        public decimal CalculateTotal()
        {
            return Price * Quantity;
        }

    }
}
