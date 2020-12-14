using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required, MaxLength(50)]
        public string FullName { get; set; }

        [Required, MaxLength(1000)]
        public string AboutMe { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [Required, MaxLength(50)]
        public string Phone { get; set; }

        [Required, MaxLength(50)]
        public string Office { get; set; }

        [Required, MaxLength(50)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string Facebook { get; set; }

        [Required, MaxLength(50)]
        public string Twitter { get; set; }

        [Required, MaxLength(50)]
        public string Linkedin { get; set; }

        [Required, MaxLength(50)]
        public string Gmail { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; }

        [NotMapped]
        public IFormFile Upload { get; set; }

        public List<Appointment.Appointment> Appointments { get; set; }

    }
}
