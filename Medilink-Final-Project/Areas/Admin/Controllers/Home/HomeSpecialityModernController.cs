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
    public class HomeSpecialityModernController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeSpecialityModernController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeSpecialityModerns.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeModern = await _context.HomeSpecialityModerns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeModern == null)
            {
                return NotFound();
            }

            return View(homeModern);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Upload,Photo,Id,Content,Text")] HomeSpecialityModern homeModern)
        {
            if (homeModern.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeModern.Upload.ContentType != "image/jpeg" && homeModern.Upload.ContentType != "image/png" && homeModern.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeModern.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeModern.Upload);
                homeModern.Photo = fileName;

                _context.Add(homeModern);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeModern);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeModern = await _context.HomeSpecialityModerns.FindAsync(id);
            if (homeModern == null)
            {
                return NotFound();
            }
            return View(homeModern);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Upload,Photo,Id,Content,Text")] HomeSpecialityModern homeModern)
        {
            if (id != homeModern.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if(homeModern.Upload != null)
                    {
                        if (homeModern.Upload.ContentType != "image/jpeg" && homeModern.Upload.ContentType != "image/png" && homeModern.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeModern);
                        }

                        if (homeModern.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeModern);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeModern.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeModern.Upload, "wwwroot/uploads");
                        homeModern.Photo = fileName;
                    }
                    

                    _context.Update(homeModern);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeModernExsist(homeModern.Id))
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
            return View(homeModern);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeModern = await _context.HomeSpecialityModerns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeModern == null)
            {
                return NotFound();
            }

            return View(homeModern);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeModern = await _context.HomeSpecialityModerns.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeModern.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeSpecialityModerns.Remove(homeModern);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeSpecialityModerns.Remove(homeModern);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeModernExsist(int id)
        {
            return _context.HomeSpecialityModerns.Any(e => e.Id == id);
        }
    }
}
