using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class AppointmentSendViewModel
    {

        public int DoctorId { get; set; }

        [Required, MaxLength(50)]
        public string PatientName { get; set; }

        [Required, MaxLength(50)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Time { get; set; }

        [Required, MaxLength(600)]
        public string Text { get; set; }
    }
}
