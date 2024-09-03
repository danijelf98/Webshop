using Webshop.Models.Dbo.UserModel;
using Webshop.Shared.Interfaces;
using Webshop.Shared.Models.Base.Common;

namespace Webshop.Models.Dbo.Common
{
    public class SessionItem : SessionItemBase, IBaseTableAtributes
    {
        public long Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool Valid { get; set; }
        public ApplicationUser? User { get; set; }
        public string? UserId { get; set; }
    }
}
