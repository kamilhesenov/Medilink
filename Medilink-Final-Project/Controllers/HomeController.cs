using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, AplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                HomeIntros = _context.HomeIntros.ToList(),
                WelcomeHospital = _context.WelcomeHospitals.FirstOrDefault(),
                HomeServices = _context.HomeServices.ToList(),
                HomeSpecialityModern = _context.HomeSpecialityModerns.FirstOrDefault(),
                HomeSpecialitySucces = _context.homeSpecialitySucces.FirstOrDefault(),
                HomeSpecialityDoctor = _context.HomeSpecialityDoctors.FirstOrDefault(),
                Doctors = _context.Doctors.Include(d => d.Department).ToList(),
                HomeHealths = _context.HomeHealths.ToList(),
                HomeCounters = _context.HomeCounters.ToList(),
                HomeTestimonials = _context.HomeTestimonials.ToList(),
                HomeNews = _context.HomeNews.ToList(),
                HomeBrands = _context.HomeBrands.ToList(),
                
            };

            return View(model);
        }

    }
}
