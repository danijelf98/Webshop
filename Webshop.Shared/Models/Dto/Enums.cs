using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Shared.Models.Dto
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Buyer = "Buyer";
    }
    public enum OrderStatus
    {
        /// <summary>
        /// Order is being accepted,
        /// </summary>
        Pending,
        /// <summary>
        /// Order is pending,
        /// </summary>
        Processing,
        /// <summary>
        /// Order is shipped,
        /// </summary>
        Shipped,
        /// <summary>
        /// Order is delivered,
        /// </summary>
        Delivered,
        /// <summary>
        /// Order is canceled,
        /// </summary>
        Canceled,
        /// <summary>
        /// Order is returned,
        /// </summary>
        Returned,
        /// <summary>
        /// Order is refunded,
        /// </summary>
        Refunded
    }
}
