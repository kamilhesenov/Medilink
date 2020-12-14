using Medilink_Final_Project.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class HomeViewModel
    {
        public List<HomeIntro> HomeIntros { get; set; }
        public WelcomeHospital WelcomeHospital { get; set; }
        public List<HomeService> HomeServices { get; set; }
        public HomeSpecialityModern HomeSpecialityModern { get; set; }
        public HomeSpecialitySucces HomeSpecialitySucces { get; set; }
        public HomeSpecialityDoctor HomeSpecialityDoctor { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<HomeHealth> HomeHealths { get; set; }
        public List<HomeCounter> HomeCounters { get; set; }
        public List<HomeTestimonial> HomeTestimonials { get; set; }
        public List<HomeNew> HomeNews { get; set; }
        public List<HomeBrand> HomeBrands { get; set; }
        
    }
}
