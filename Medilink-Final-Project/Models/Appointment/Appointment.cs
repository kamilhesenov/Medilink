using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.Appointment
{
    public class Appointment
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }
    }
}
