using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Market.Models.Tables
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public int Customer_Id { get; set; }
        public virtual Customer Customer { get; set; }
        [Required]
        public DateTime Order_Date { get; set; }
        public DateTime Shipment_Date { get; set; }
        public int Order_Number { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
