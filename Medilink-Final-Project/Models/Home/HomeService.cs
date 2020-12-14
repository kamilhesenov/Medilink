using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.Home
{
    public class HomeService
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Required, MaxLength(200)]
        public string Content { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }
    }
}
