using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class BasketViewModel
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int DbCount { get; set; }
        public string UserName { get; set; }
        public double ProductTotalPrice { get; set; }
        public int BasketCount { get; set; } = 1;
    }
}
