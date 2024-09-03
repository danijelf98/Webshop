using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.Common;

namespace Webshop.Shared.Models.ViewModel.Common
{
    public class AddressViewModel : AddressBase
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
    }
}
