using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class DoctorSearchViewModel
    {
        public BannerViewModel BannerViewModel { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
