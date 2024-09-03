using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Base.Common;

namespace Webshop.Shared.Models.Binding.Common
{
    public class AddressBinding : AddressBase
    {
    }
    public class AddressUpdateBinding : AddressBase
    {
        public long Id { get; set; }
    }
}
