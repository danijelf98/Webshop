using System.ComponentModel.DataAnnotations;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Binding.ProductModels;

namespace Webshop.Models.Dbo.ProductModels
{
    public class QuantityType : QuantityTypeBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }
    }
}
