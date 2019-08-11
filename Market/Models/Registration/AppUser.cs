using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Market.Models.Tables;

namespace Market.Models.Registration
{
    public class AppUser : IdentityUser
    {
        public int? Customer_Id { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
