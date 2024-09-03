using System.ComponentModel.DataAnnotations;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.Common;

namespace Webshop.Models.Dbo.Common
{
    public class Address : AddressBase, IBaseTableAtributes
    {
        [Key]
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }
    }
}
