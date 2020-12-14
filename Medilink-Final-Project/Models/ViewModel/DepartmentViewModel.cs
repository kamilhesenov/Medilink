using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class DepartmentViewModel
    {
        public BannerViewModel BannerViewModel { get; set; }

        
        public DepartmentPaginationViewModel DepartmentPaginationViewModel { get; set; }
    }
}
