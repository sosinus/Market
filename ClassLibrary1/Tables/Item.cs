using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Tables
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        [StringLength(30)]
        public string Category { get; set; }
        [NotMapped]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [NotMapped]
        public string Image { get; set; }
    }
}
