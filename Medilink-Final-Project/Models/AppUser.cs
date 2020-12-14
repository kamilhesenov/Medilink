using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }
        
    }
}
