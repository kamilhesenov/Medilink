using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class AppointmentViewModel
    {
        public BannerViewModel BannerViewModel { get; set; }
        public Appointment.AppointmentPhoto AppointmentPhoto { get; set; }
        public Appointment.Appointment Appointment { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Department> Departments { get; set; }
        public AppointmentSendViewModel AppointmentSendViewModel { get; set; }
    }
}
