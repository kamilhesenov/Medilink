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
    public class LabaratoryController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;
        public LabaratoryController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Labaratories.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labaratory = await _context.Labaratories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labaratory == null)
            {
                return NotFound();
            }

            return View(labaratory);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Heading,Title,Upload,Photo,Id,Content,Subject,Text")] Labaratory labaratory)
        {
            if (labaratory.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (labaratory.Upload.ContentType != "image/jpeg" && labaratory.Upload.ContentType != "image/png" && labaratory.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (labaratory.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(labaratory.Upload);
                labaratory.Photo = fileName;

                _context.Add(labaratory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labaratory);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labaratory = await _context.Labaratories.FindAsync(id);
            if (labaratory == null)
            {
                return NotFound();
            }
            return View(labaratory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Heading,Title,Upload,Photo,Id,Content,Subject,Text")] Labaratory labaratory)
        {
            if (id != labaratory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (labaratory.Upload != null)
                    {
                        if (labaratory.Upload.ContentType != "image/jpeg" && labaratory.Upload.ContentType != "image/png" && labaratory.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(labaratory);
                        }

                        if (labaratory.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(labaratory);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", labaratory.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(labaratory.Upload, "wwwroot/uploads");
                        labaratory.Photo = fileName;
                    }


                    _context.Update(labaratory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabaratoryExsist(labaratory.Id))
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
            return View(labaratory);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labaratory = await _context.Labaratories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labaratory == null)
            {
                return NotFound();
            }

            return View(labaratory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labaratory = await _context.Labaratories.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", labaratory.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.Labaratories.Remove(labaratory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.Labaratories.Remove(labaratory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabaratoryExsist(int id)
        {
            return _context.Labaratories.Any(e => e.Id == id);
        }
    }
}
