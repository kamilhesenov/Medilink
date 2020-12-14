using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers.Home
{
    [Area("Admin")]
    public class HomeSpecialityDoctorController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeSpecialityDoctorController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeSpecialityDoctors.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeDoctor = await _context.HomeSpecialityDoctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeDoctor == null)
            {
                return NotFound();
            }

            return View(homeDoctor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Upload,Photo,Id,Content")] HomeSpecialityDoctor homeDoctor)
        {
            if (homeDoctor.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeDoctor.Upload.ContentType != "image/jpeg" && homeDoctor.Upload.ContentType != "image/png" && homeDoctor.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeDoctor.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeDoctor.Upload);
                homeDoctor.Photo = fileName;

                _context.Add(homeDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeDoctor);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeDoctor = await _context.HomeSpecialityDoctors.FindAsync(id);
            if (homeDoctor == null)
            {
                return NotFound();
            }
            return View(homeDoctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Upload,Photo,Id,Content")] HomeSpecialityDoctor homeDoctor)
        {
            if (id != homeDoctor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (homeDoctor.Upload != null)
                    {
                        if (homeDoctor.Upload.ContentType != "image/jpeg" && homeDoctor.Upload.ContentType != "image/png" && homeDoctor.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeDoctor);
                        }

                        if (homeDoctor.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeDoctor);
                        }
                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeDoctor.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeDoctor.Upload, "wwwroot/uploads");
                        homeDoctor.Photo = fileName;
                    }


                    _context.Update(homeDoctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeDoctorExsist(homeDoctor.Id))
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
            return View(homeDoctor);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeDoctor = await _context.HomeSpecialityDoctors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeDoctor == null)
            {
                return NotFound();
            }

            return View(homeDoctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeDoctor = await _context.HomeSpecialityDoctors.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeDoctor.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeSpecialityDoctors.Remove(homeDoctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeSpecialityDoctors.Remove(homeDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeDoctorExsist(int id)
        {
            return _context.HomeSpecialityDoctors.Any(e => e.Id == id);
        }
    }
}
