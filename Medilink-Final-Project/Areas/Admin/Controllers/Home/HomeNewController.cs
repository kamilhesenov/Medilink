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
    public class HomeNewController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeNewController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.HomeNews.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeNews = await _context.HomeNews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeNews == null)
            {
                return NotFound();
            }

            return View(homeNews);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Upload,Photo,Id,Content,Date,Icon,Count")] HomeNew homeNews)
        {
            if (homeNews.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (homeNews.Upload.ContentType != "image/jpeg" && homeNews.Upload.ContentType != "image/png" && homeNews.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (homeNews.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(homeNews.Upload);
                homeNews.Photo = fileName;

                _context.Add(homeNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeNews);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeNews = await _context.HomeNews.FindAsync(id);
            if (homeNews == null)
            {
                return NotFound();
            }
            return View(homeNews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Upload,Photo,Id,Content,Date,Icon,Count")] HomeNew homeNews)
        {
            if (id != homeNews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (homeNews.Upload != null)
                    {
                        if (homeNews.Upload.ContentType != "image/jpeg" && homeNews.Upload.ContentType != "image/png" && homeNews.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(homeNews);
                        }

                        if (homeNews.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(homeNews);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeNews.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(homeNews.Upload, "wwwroot/uploads");
                        homeNews.Photo = fileName;

                    }

                    _context.Update(homeNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeNewsExsist(homeNews.Id))
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
            return View(homeNews);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeNews = await _context.HomeNews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeNews == null)
            {
                return NotFound();
            }

            return View(homeNews);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeNews = await _context.HomeNews.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", homeNews.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.HomeNews.Remove(homeNews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.HomeNews.Remove(homeNews);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeNewsExsist(int id)
        {
            return _context.HomeNews.Any(e => e.Id == id);
        }
    }
}
