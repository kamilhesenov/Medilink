using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class DoctorSearchController : Controller
    {
        private readonly AplicationDbContext _context;
        public DoctorSearchController(AplicationDbContext context)
        {
            _context = context;
        }

        
        [Route("doctors")]
        public IActionResult Index(int departmentId,string name)
        {
            ViewBag.Departments = _context.Doctors.Include(d => d.Department).ToList();
            DoctorSearchViewModel model = new DoctorSearchViewModel();

            BannerViewModel bannerView = new BannerViewModel
            {
                Title = "Doctors Search"
            };
            model.BannerViewModel = bannerView;


            if (departmentId != 0)
            {
                model.Doctors = _context.Doctors.Include(d => d.Department).Where(d=>d.DepartmentId == departmentId).ToList();

                return View(model);
            }
            if (departmentId != 0 && name != null)
            {
                model.Doctors = _context.Doctors.Include(d => d.Department)
                    .Where(d => d.DepartmentId == departmentId && d.FullName.ToLower().Contains(name.ToLower())).ToList();

                return View(model);
            }
            if (name != null)
            {
                model.Doctors = _context.Doctors.Include(d => d.Department).Where(d => d.FullName.ToLower().Contains(name.ToLower())).ToList();

                return View(model);
            }
            else
            {
                model.Doctors = _context.Doctors.Include(d => d.Department).ToList();

                return View(model);
            }
            
        }

    }
}
