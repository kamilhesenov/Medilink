using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AplicationDbContext _context;
        public DepartmentController(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1,int pageSize = 4)
        {
            var items = _context.Departments.AsNoTracking().OrderBy(x => x.Id);
            var pagingData = await PagingList.CreateAsync(items, pageSize, page);

            DepartmentViewModel model = new DepartmentViewModel
            {
                
                DepartmentPaginationViewModel = new DepartmentPaginationViewModel
                {
                    PagingList = pagingData
                },
                BannerViewModel = new BannerViewModel
                {
                    Title = "Departments"
                },
                

            };

            return View(model);
        }
    }
}
