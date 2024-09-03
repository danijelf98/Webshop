using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Shared.Models.Binding.Common;

namespace Webshop.Shared.Models.Binding.Account_Models
{
    public class ApplicationUserUpdateBinding
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressUpdateBinding? Address { get; set; }
    }
}
