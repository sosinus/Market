using Market.Models.Registration;
using Market.Models.Tables;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Имя не должно превышать 50 символов")]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string Address { get; set; }
        public int Discount { get; set; }
        public virtual IEnumerable<Order> Order { get; set; }
        public virtual AppUser AppUser { get; set; }

    }
}
