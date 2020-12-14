using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class ShopCkeckout
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int CheckoutId { get; set; }
        public Shop Shop { get; set; }
        public Checkout Checkout { get; set; }

        [Column(TypeName = "money")]
        public double Price { get; set; }

        public int Count { get; set; }
        
    }
}
