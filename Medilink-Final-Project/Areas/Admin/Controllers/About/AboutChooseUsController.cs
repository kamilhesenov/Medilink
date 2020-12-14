using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers.About
{
    [Area("Admin")]
    public class AboutChooseUsController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public AboutChooseUsController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.AboutChooseUs.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutChooseUs = await _context.AboutChooseUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutChooseUs == null)
            {
                return NotFound();
            }

            return View(aboutChooseUs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Upload,Photo,Id")] AboutChooseUs aboutChooseUs)
        {
            if (aboutChooseUs.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (aboutChooseUs.Upload.ContentType != "image/jpeg" && aboutChooseUs.Upload.ContentType != "image/png" && aboutChooseUs.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (aboutChooseUs.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(aboutChooseUs.Upload);
                aboutChooseUs.Photo = fileName;

                _context.Add(aboutChooseUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutChooseUs);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutChooseUs = await _context.AboutChooseUs.FindAsync(id);
            if (aboutChooseUs == null)
            {
                return NotFound();
            }
            return View(aboutChooseUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Upload,Photo,Id")] AboutChooseUs aboutChooseUs)
        {
            if (id != aboutChooseUs.Id)
            {
                return NotFound();
            }

            if (aboutChooseUs.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            if (ModelState.IsValid)
            {

                try
                {
                    if (aboutChooseUs.Upload != null)
                    {
                        if (aboutChooseUs.Upload.ContentType != "image/jpeg" && aboutChooseUs.Upload.ContentType != "image/png" && aboutChooseUs.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(aboutChooseUs);
                        }

                        if (aboutChooseUs.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(aboutChooseUs);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", aboutChooseUs.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(aboutChooseUs.Upload, "wwwroot/uploads");
                        aboutChooseUs.Photo = fileName;

                    }

                    _context.Update(aboutChooseUs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutChooseUsExsist(aboutChooseUs.Id))
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
            return View(aboutChooseUs);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutChooseUs = await _context.AboutChooseUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutChooseUs == null)
            {
                return NotFound();
            }

            return View(aboutChooseUs);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutChooseUs = await _context.AboutChooseUs.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", aboutChooseUs.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.AboutChooseUs.Remove(aboutChooseUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.AboutChooseUs.Remove(aboutChooseUs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutChooseUsExsist(int id)
        {
            return _context.AboutChooseUs.Any(e => e.Id == id);
        }
    }
}
