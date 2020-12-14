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
    public class HomeCounterController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeCounterController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeCounters.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeCounter = await _context.HomeCounters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeCounter == null)
            {
                return NotFound();
            }

            return View(homeCounter);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Count,Upload,Photo,Id,Content")] HomeCounter homeCounter)
        {
            if (homeCounter.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeCounter.Upload.ContentType != "image/jpeg" && homeCounter.Upload.ContentType != "image/png" && homeCounter.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeCounter.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeCounter.Upload);
                homeCounter.Photo = fileName;

                _context.Add(homeCounter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeCounter);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeCounter = await _context.HomeCounters.FindAsync(id);
            if (homeCounter == null)
            {
                return NotFound();
            }
            return View(homeCounter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Count,Upload,Photo,Id,Content")] HomeCounter homeCounter)
        {
            if (id != homeCounter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (homeCounter.Upload != null)
                    {
                        if (homeCounter.Upload.ContentType != "image/jpeg" && homeCounter.Upload.ContentType != "image/png" && homeCounter.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeCounter);
                        }

                        if (homeCounter.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeCounter);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeCounter.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeCounter.Upload, "wwwroot/uploads");
                        homeCounter.Photo = fileName;

                    }

                    _context.Update(homeCounter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeCounterExsist(homeCounter.Id))
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
            return View(homeCounter);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeCounter = await _context.HomeCounters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeCounter == null)
            {
                return NotFound();
            }

            return View(homeCounter);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeCounter = await _context.HomeCounters.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeCounter.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeCounters.Remove(homeCounter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeCounters.Remove(homeCounter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeCounterExsist(int id)
        {
            return _context.HomeCounters.Any(e => e.Id == id);
        }
    }
}
