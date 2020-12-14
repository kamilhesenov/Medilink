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
    public class HomeIntroesController : Controller
    {

        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;
        public HomeIntroesController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeIntros.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeIntro = await _context.HomeIntros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeIntro == null)
            {
                return NotFound();
            }

            return View(homeIntro);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Header,Upload,Photo,Id,Content")] HomeIntro homeIntro)
        {
            if (homeIntro.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeIntro.Upload.ContentType != "image/jpeg" && homeIntro.Upload.ContentType != "image/png" && homeIntro.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeIntro.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeIntro.Upload);
                homeIntro.Photo = fileName;

                _context.Add(homeIntro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeIntro);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeIntro = await _context.HomeIntros.FindAsync(id);
            if (homeIntro == null)
            {
                return NotFound();
            }
            return View(homeIntro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Header,Upload,Photo,Id,Content")] HomeIntro homeIntro)
        {
            if (id != homeIntro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if(homeIntro.Upload != null)
                    {
                        if (homeIntro.Upload.ContentType != "image/jpeg" && homeIntro.Upload.ContentType != "image/png" && homeIntro.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeIntro);
                        }

                        if (homeIntro.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeIntro);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeIntro.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeIntro.Upload, "wwwroot/uploads");
                        homeIntro.Photo = fileName;
                    }
                    
                    _context.Update(homeIntro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeIntroExsist(homeIntro.Id))
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
            return View(homeIntro);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeIntro = await _context.HomeIntros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeIntro == null)
            {
                return NotFound();
            }

            return View(homeIntro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeIntro = await _context.HomeIntros.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeIntro.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeIntros.Remove(homeIntro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            _context.HomeIntros.Remove(homeIntro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeIntroExsist(int id)
        {
            return _context.HomeIntros.Any(e => e.Id == id);
        }
    }
}
