using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class CheckoutViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        public string Phone { get; set; }
        public BannerViewModel BannerViewModel { get; set; }

        public Checkout Checkout { get; set; }

    }
}
