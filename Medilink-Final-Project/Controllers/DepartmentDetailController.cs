using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class DepartmentDetailController : Controller
    {
        private readonly AplicationDbContext _context;
        public DepartmentDetailController(AplicationDbContext context)
        {
            _context = context;
        }

        [Route("department/detail")]
        public IActionResult Index(int Id)
        {
            DepartmentDetailViewModel model = new DepartmentDetailViewModel
            {
                Department = _context.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.Id == Id),
                Departments = _context.Departments.ToList()
            };
            

            
            return View(model);
        }
    }
}
