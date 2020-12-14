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
    public class AboutController : Controller
    {
        private readonly AplicationDbContext _context;
        public AboutController(AplicationDbContext context)
        {
            _context = context;
        }

        [Route("about")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
        {
            var items = _context.Departments.AsNoTracking().OrderBy(x => x.Id);
            var pagingData = await PagingList.CreateAsync(items, pageSize, page);
            AboutViewModel model = new AboutViewModel
            {
                
                DepartmentPaginationViewModel = new DepartmentPaginationViewModel
                {
                    PagingList = pagingData
                },
                Labaratory = _context.Labaratories.FirstOrDefault(),
                HomeCounters = _context.HomeCounters.ToList(),
                AboutChooseUs = _context.AboutChooseUs.FirstOrDefault(),
                AboutChooseUsItems = _context.AboutChooseUsItems.ToList(),
                HomeTestimonials = _context.HomeTestimonials.ToList(),
                HomeBrands = _context.HomeBrands.ToList(),
                BannerViewModel = new BannerViewModel
                {
                    Title = "About"
                }
            };
            return View(model);
        }

        
    }
}
