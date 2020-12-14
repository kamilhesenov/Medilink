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
    public class HomeHealthController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeHealthController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeHealths.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeHelth = await _context.HomeHealths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeHelth == null)
            {
                return NotFound();
            }

            return View(homeHelth);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Upload,Photo,Id,Content")] HomeHealth homeHelth)
        {
            if (homeHelth.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeHelth.Upload.ContentType != "image/jpeg" && homeHelth.Upload.ContentType != "image/png" && homeHelth.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeHelth.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeHelth.Upload);
                homeHelth.Photo = fileName;

                _context.Add(homeHelth);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeHelth);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeHelth = await _context.HomeHealths.FindAsync(id);
            if (homeHelth == null)
            {
                return NotFound();
            }
            return View(homeHelth);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Upload,Photo,Id,Content")] HomeHealth homeHelth)
        {
            if (id != homeHelth.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (homeHelth.Upload != null)
                    {
                        if (homeHelth.Upload.ContentType != "image/jpeg" && homeHelth.Upload.ContentType != "image/png" && homeHelth.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeHelth);
                        }

                        if (homeHelth.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeHelth);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeHelth.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeHelth.Upload, "wwwroot/uploads");
                        homeHelth.Photo = fileName;

                    }

                    _context.Update(homeHelth);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeHealthExsist(homeHelth.Id))
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
            return View(homeHelth);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeHelth = await _context.HomeHealths
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeHelth == null)
            {
                return NotFound();
            }

            return View(homeHelth);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeHelth = await _context.HomeHealths.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeHelth.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeHealths.Remove(homeHelth);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeHealths.Remove(homeHelth);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeHealthExsist(int id)
        {
            return _context.HomeHealths.Any(e => e.Id == id);
        }
    }
}
