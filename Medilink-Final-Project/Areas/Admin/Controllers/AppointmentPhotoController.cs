using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models.Appointment;
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
    public class AppointmentPhotoController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public AppointmentPhotoController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppointmentPhotos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentPhoto = await _context.AppointmentPhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentPhoto == null)
            {
                return NotFound();
            }

            return View(appointmentPhoto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Upload,Photo,Id")] AppointmentPhoto appointmentPhoto)
        {
            if (appointmentPhoto.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (appointmentPhoto.Upload.ContentType != "image/jpeg" && appointmentPhoto.Upload.ContentType != "image/png" && appointmentPhoto.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (appointmentPhoto.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(appointmentPhoto.Upload);
                appointmentPhoto.Photo = fileName;

                _context.Add(appointmentPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentPhoto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentPhoto = await _context.AppointmentPhotos.FindAsync(id);
            if (appointmentPhoto == null)
            {
                return NotFound();
            }
            return View(appointmentPhoto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Upload,Photo,Id")] AppointmentPhoto appointmentPhoto)
        {
            if (id != appointmentPhoto.Id)
            {
                return NotFound();
            }

            if (appointmentPhoto.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (appointmentPhoto.Upload != null)
                    {
                        if (appointmentPhoto.Upload.ContentType != "image/jpeg" && appointmentPhoto.Upload.ContentType != "image/png" && appointmentPhoto.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(appointmentPhoto);
                        }


                        if (appointmentPhoto.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(appointmentPhoto);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", appointmentPhoto.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(appointmentPhoto.Upload, "wwwroot/uploads");
                        appointmentPhoto.Photo = fileName;

                    }

                    _context.Update(appointmentPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentPhotoExsist(appointmentPhoto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentPhoto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentPhoto = await _context.AppointmentPhotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentPhoto == null)
            {
                return NotFound();
            }

            return View(appointmentPhoto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentPhoto = await _context.AppointmentPhotos.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", appointmentPhoto.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.AppointmentPhotos.Remove(appointmentPhoto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.AppointmentPhotos.Remove(appointmentPhoto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentPhotoExsist(int id)
        {
            return _context.HomeBrands.Any(e => e.Id == id);
        }
    }
}
