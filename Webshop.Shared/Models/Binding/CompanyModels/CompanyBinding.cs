using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.CompanyModels;
using Webshop.Shared.Models.Binding.Common;

namespace Webshop.Shared.Models.Binding.CompanyModels
{
    public class CompanyUpdateBinding : CompanyBase
    {
        public long Id { get; set; }
        public AddressUpdateBinding? Address { get; set; }
    }
}
