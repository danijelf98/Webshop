using System.ComponentModel.DataAnnotations;
using Webshop.Models.Dbo.Common;
using Webshop.Models.Dbo.ProductModels;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.CompanyModels;

namespace Webshop.Models.Dbo.CompanyModels
{
    public class Company : CompanyBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }

        public Address? Address { get; set; }
        public long? AddressId { get; set; }
        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}
