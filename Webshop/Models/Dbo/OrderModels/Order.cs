using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Webshop.Models.Dbo.Common;
using Webshop.Models.Dbo.UserModel;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.OrderModels;
using Webshop.Shared.Models.Dto;

namespace Webshop.Models.Dbo.OrderModels
{
    public class Order : OrderBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }
        [Required(ErrorMessage = "Total Price is Required.")]
        [Column(TypeName = "decimal(7, 2)")]
        public decimal Total { get; set; }
        public ApplicationUser? Buyer { get; set; }
        public string? BuyerId { get; set; }
        public Address? OrderAddress { get; set; }
        public long? OrderAddressId { get; set; }
        public OrderStatus? OrderStatus { get; set; }

        public ICollection<BuyerFeedback>? BuyerFeedbacks { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }

        public void CalculateTotal()
        {
            if (OrderItems == null)
            {
                return;
            }

            Total = OrderItems.Select(y => y.CalculateTotal()).Sum();
        }
    }
}
