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
    public class HomeServicesController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeServicesController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeServices.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeService = await _context.HomeServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeService == null)
            {
                return NotFound();
            }

            return View(homeService);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Upload,Photo,Id,Content")] HomeService homeService)
        {
            if (homeService.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeService.Upload.ContentType != "image/jpeg" && homeService.Upload.ContentType != "image/png" && homeService.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeService.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeService.Upload);
                homeService.Photo = fileName;

                _context.Add(homeService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeService);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeService = await _context.HomeServices.FindAsync(id);
            if (homeService == null)
            {
                return NotFound();
            }
            return View(homeService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Upload,Photo,Id,Content")] HomeService homeService)
        {
            if (id != homeService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if(homeService.Upload != null)
                    {
                        if (homeService.Upload.ContentType != "image/jpeg" && homeService.Upload.ContentType != "image/png" && homeService.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeService);
                        }

                        if (homeService.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeService);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeService.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeService.Upload, "wwwroot/uploads");
                        homeService.Photo = fileName;

                    }

                    _context.Update(homeService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeServiceExsist(homeService.Id))
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
            return View(homeService);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeService = await _context.HomeServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeService == null)
            {
                return NotFound();
            }

            return View(homeService);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeService = await _context.HomeServices.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeService.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeServices.Remove(homeService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeServices.Remove(homeService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeServiceExsist(int id)
        {
            return _context.HomeServices.Any(e => e.Id == id);
        }
    }
}
