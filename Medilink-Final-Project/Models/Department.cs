using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Medilink_Final_Project.Models.Appointment;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [Required, MaxLength(650)]
        public string Content { get; set; }

        [Required, MaxLength(650)]
        public string Advantage { get; set; }

        [Required, MaxLength(750)]
        public string Text { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }
        public List<Doctor> Doctors { get; set; }
        
    }
}
