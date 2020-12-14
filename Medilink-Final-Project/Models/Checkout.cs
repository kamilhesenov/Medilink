using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public List<ShopCkeckout> ShopCkeckouts { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }


    }
}
