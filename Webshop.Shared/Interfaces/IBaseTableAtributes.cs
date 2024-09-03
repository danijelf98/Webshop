using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Interfaces
{
    public interface IBaseTableAtributes
    {
        long Id { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
        bool Valid { get; set; }
    }
}
