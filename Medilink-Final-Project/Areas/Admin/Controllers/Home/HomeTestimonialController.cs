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
    public class HomeTestimonialController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeTestimonialController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeTestimonials.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeTestimonial = await _context.HomeTestimonials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeTestimonial == null)
            {
                return NotFound();
            }

            return View(homeTestimonial);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Upload,Photo,Id,Profession,Description")] HomeTestimonial homeTestimonial)
        {
            if (homeTestimonial.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeTestimonial.Upload.ContentType != "image/jpeg" && homeTestimonial.Upload.ContentType != "image/png" && homeTestimonial.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeTestimonial.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeTestimonial.Upload);
                homeTestimonial.Photo = fileName;

                _context.Add(homeTestimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeTestimonial);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeTestimonial = await _context.HomeTestimonials.FindAsync(id);
            if (homeTestimonial == null)
            {
                return NotFound();
            }
            return View(homeTestimonial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FullName,Upload,Photo,Id,Profession,Description")] HomeTestimonial homeTestimonial)
        {
            if (id != homeTestimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (homeTestimonial.Upload != null)
                    {
                        if (homeTestimonial.Upload.ContentType != "image/jpeg" && homeTestimonial.Upload.ContentType != "image/png" && homeTestimonial.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeTestimonial);
                        }

                        if (homeTestimonial.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeTestimonial);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeTestimonial.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeTestimonial.Upload, "wwwroot/uploads");
                        homeTestimonial.Photo = fileName;

                    }

                    _context.Update(homeTestimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeTestimonialExsist(homeTestimonial.Id))
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
            return View(homeTestimonial);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeTestimonial = await _context.HomeTestimonials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeTestimonial == null)
            {
                return NotFound();
            }

            return View(homeTestimonial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeTestimonial = await _context.HomeTestimonials.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeTestimonial.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeTestimonials.Remove(homeTestimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeTestimonials.Remove(homeTestimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeTestimonialExsist(int id)
        {
            return _context.HomeTestimonials.Any(e => e.Id == id);
        }
    }
}
