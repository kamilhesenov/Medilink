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
    public class HomeBrandController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeBrandController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeBrands.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeBrand = await _context.HomeBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeBrand == null)
            {
                return NotFound();
            }

            return View(homeBrand);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Upload,Photo,Id")] HomeBrand homeBrand)
        {
            if (homeBrand.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeBrand.Upload.ContentType != "image/jpeg" && homeBrand.Upload.ContentType != "image/png" && homeBrand.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeBrand.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeBrand.Upload);
                homeBrand.Photo = fileName;

                _context.Add(homeBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeBrand);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeBrand = await _context.HomeBrands.FindAsync(id);
            if (homeBrand == null)
            {
                return NotFound();
            }
            return View(homeBrand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Upload,Photo,Id")] HomeBrand homeBrand)
        {
            if (id != homeBrand.Id)
            {
                return NotFound();
            }

            if (homeBrand.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (homeBrand.Upload != null)
                    {
                        if (homeBrand.Upload.ContentType != "image/jpeg" && homeBrand.Upload.ContentType != "image/png" && homeBrand.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeBrand);
                        }


                        if (homeBrand.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeBrand);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeBrand.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeBrand.Upload, "wwwroot/uploads");
                        homeBrand.Photo = fileName;

                    }

                    _context.Update(homeBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeBrandExsist(homeBrand.Id))
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
            return View(homeBrand);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeBrand = await _context.HomeBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeBrand == null)
            {
                return NotFound();
            }

            return View(homeBrand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeBrand = await _context.HomeBrands.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeBrand.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeBrands.Remove(homeBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeBrands.Remove(homeBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeBrandExsist(int id)
        {
            return _context.HomeBrands.Any(e => e.Id == id);
        }
    }
}
