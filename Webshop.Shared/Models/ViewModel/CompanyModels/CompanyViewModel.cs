using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.CompanyModels;
using Webshop.Shared.Models.ViewModel.Common;

namespace Webshop.Shared.Models.ViewModel.CompanyModels
{
    public class CompanyViewModel : CompanyBase
    {
        public long Id { get; set; }
        public AddressViewModel? Address { get; set; }
        public long? AddressId { get; set; }
    }
}
