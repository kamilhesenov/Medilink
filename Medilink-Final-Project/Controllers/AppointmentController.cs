using Medilink_Final_Project.Data;
using Medilink_Final_Project.Models.Appointment;
using Medilink_Final_Project.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Controllers

    
{
    public class AppointmentController : Controller
    {
        private readonly AplicationDbContext _context;
        public AppointmentController(AplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            AppointmentViewModel model = new AppointmentViewModel
            {
                AppointmentPhoto = _context.AppointmentPhotos.FirstOrDefault(),
                BannerViewModel = new BannerViewModel
                {
                    Title = "Appointment Form"
                },
                Doctors = _context.Doctors.ToList(),
                Departments = _context.Departments.Include(d=>d.Doctors).ToList()
            };
            return View(model);
        }

        
        public IActionResult GetDoctorsByDepartment(int? id)
        {
            if (id == null) return RedirectToAction("Index", "Error404");
            var doctors = _context.Doctors.Where(d => d.DepartmentId == id).ToList();
            return PartialView("_getDoctor",doctors);
        }

        [HttpPost]
        public IActionResult Send(AppointmentSendViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            Appointment appointment = new Appointment
                    {

                        DoctorId = model.DoctorId,
                        PatientName = model.PatientName,
                        Phone = model.Phone,
                        Email = model.Email,
                        Date = model.Date,
                        Text = model.Text
                    };

                    _context.Appointments.Add(appointment);
                    _context.SaveChanges();

            return NoContent();

          }
    }
}
