using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Views.ViewModel
{
    public class ShopCreateViewModel
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int Count { get; set; }


        public double Price { get; set; }
        public byte Star { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }
    }
}
