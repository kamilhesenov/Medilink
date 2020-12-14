using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.Home
{
    public class HomeNew
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required, MaxLength(50)]
        public string Icon { get; set; }

        [Required]
        public int Count { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }
    }
}
