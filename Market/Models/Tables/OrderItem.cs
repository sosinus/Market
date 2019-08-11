using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Models.Tables
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        public int Order_Id { get; set; }
        public virtual Order Order { get; set; }
        [Required]
        public int Item_Id { get; set; }
        public virtual Item Item { get; set; }
        [Required]
        public int Items_count { get; set; }
        public double Item_Price { get; set; }

    }

}
