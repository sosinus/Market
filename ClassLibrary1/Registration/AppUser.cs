using Microsoft.AspNetCore.Identity;
using Models.Tables;


namespace Models.Registration
{
    public class AppUser : IdentityUser
    {
        public int? Customer_Id { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
