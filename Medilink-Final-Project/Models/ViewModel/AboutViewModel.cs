using Medilink_Final_Project.Models.About;
using Medilink_Final_Project.Models.Home;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class AboutViewModel
    {
        public BannerViewModel BannerViewModel { get; set; }
        public List<HomeCounter> HomeCounters { get; set; }
        public Labaratory Labaratory { get; set; }
        public AboutChooseUs AboutChooseUs { get; set; }
        public List<AboutChooseUsItem> AboutChooseUsItems { get; set; }
        public List<HomeTestimonial> HomeTestimonials { get; set; }
        public List<HomeBrand> HomeBrands { get; set; }

        public DepartmentPaginationViewModel DepartmentPaginationViewModel { get; set; }
    }
}
