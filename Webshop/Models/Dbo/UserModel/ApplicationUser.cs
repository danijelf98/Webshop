using Microsoft.AspNetCore.Identity;
using Webshop.Models.Dbo.Common;

namespace Webshop.Models.Dbo.UserModel
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address? Address { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
