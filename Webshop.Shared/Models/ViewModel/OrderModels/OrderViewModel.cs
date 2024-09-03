using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.OrderModels;
using Webshop.Shared.Models.Dto;
using Webshop.Shared.Models.ViewModel.Common;
using Webshop.Shared.Models.ViewModel.ProductModels;
using Webshop.Shared.Models.ViewModel.UserModels;

namespace Webshop.Shared.Models.ViewModel.OrderModels
{
    public class OrderViewModel : OrderBase
    {
        [Display(Name = "Order Id")]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public ApplicationUserViewModel? Buyer { get; set; }
        public AddressViewModel? OrderAddress { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public List<OrderItemViewModel>? OrderItems { get; set; }
        [Required(ErrorMessage = "Total price is required.")]
        [Column(TypeName = "decimal(7,2)")]
        public decimal Total { get; set; }
        public List<BuyerFeedbackViewModel>? BuyerFeedbacks { get; set; }
    }
}
