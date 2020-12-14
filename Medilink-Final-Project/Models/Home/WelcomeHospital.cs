using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.Home
{
    public class WelcomeHospital
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Heading { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }

        [MaxLength(100)]
        public string SecondPhoto { get; set; }

        [NotMapped]
        public IFormFile SecondUpload { get; set; }
    }
}
