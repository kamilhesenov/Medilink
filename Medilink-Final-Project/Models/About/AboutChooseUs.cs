using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.About
{
    public class AboutChooseUs
    {
        public int Id { get; set; }

        [MaxLength(150)]
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }
    }
}
