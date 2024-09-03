using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.Base.CompanyModels
{
    public abstract class CompanyBase
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string VAT { get; set; }
    }
}
