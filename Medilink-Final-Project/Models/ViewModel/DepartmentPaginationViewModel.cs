using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Models.ViewModel
{
    public class DepartmentPaginationViewModel
    {
        public PagingList<Department> PagingList { get; set; }
    }
}
