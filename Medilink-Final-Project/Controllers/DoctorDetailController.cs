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
    public class DoctorDetailController : Controller
    {
        private readonly AplicationDbContext _context;
        public DoctorDetailController(AplicationDbContext context)
        {
            _context = context;
        }

        [Route("doctor/detail")]
        public IActionResult Index(int Id)
        {
            DoctorDetailViewModel model = new DoctorDetailViewModel
            {
                Doctor = _context.Doctors.Include(d => d.Department).FirstOrDefault(d => d.Id == Id)
            };
            
           return View(model);
        }
    }
}
