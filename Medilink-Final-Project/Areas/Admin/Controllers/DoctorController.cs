using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public DoctorController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            var doctors = await _context.Doctors.Include(d => d.Department).ToListAsync();
            return View(doctors);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var doctor = await _context.Doctors.Include(d => d.Department).FirstOrDefaultAsync(d => d.Id == id);

            if (doctor == null) return NotFound();

            return View(doctor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            if (doctor.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (doctor.Upload.ContentType != "image/jpeg" && doctor.Upload.ContentType != "image/png" && doctor.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (doctor.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }
            }

            if (ModelState.IsValid)
            {
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + Path.GetExtension(doctor.Upload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    doctor.Upload.CopyTo(stream);
                }

                doctor.Photo = fileName;

                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.Departments = _context.Departments.ToList();

            return View(doctor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.Departments = _context.Departments.ToList();

            return View(doctor);
        }

        [HttpPost]
        public IActionResult Edit(Doctor doctor)
        {
            if (doctor.Upload != null)
            {
                if (doctor.Upload.ContentType != "image/jpeg" && doctor.Upload.ContentType != "image/png" && doctor.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (doctor.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }
            }

            if (ModelState.IsValid)
            {
                if (doctor.Upload != null)
                {
                    var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", doctor.Photo);

                    System.IO.File.Delete(oldFile);

                    var fileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + Path.GetExtension(doctor.Upload.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        doctor.Upload.CopyTo(stream);
                    }

                    doctor.Photo = fileName;
                }

                _context.Entry(doctor).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Departments = _context.Departments.ToList();

            return View(doctor);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.Include(d=>d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", doctor.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
