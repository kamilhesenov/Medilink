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
    public class WelcomeHospitalController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public WelcomeHospitalController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.WelcomeHospitals.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var welcome = await _context.WelcomeHospitals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (welcome == null)
            {
                return NotFound();
            }

            return View(welcome);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Heading,Upload,Photo,Id,Content,SecondUpload,SecondPhoto")] WelcomeHospital welcome)
        {
            if (welcome.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (welcome.Upload.ContentType != "image/jpeg" && welcome.Upload.ContentType != "image/png" && welcome.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (welcome.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }
            }
            if(welcome.SecondUpload == null)
            {
                ModelState.AddModelError("SecondUpload", "Şəkil məcburidir");
            }
            else
            {
                if (welcome.SecondUpload.ContentType != "image/jpeg" && welcome.SecondUpload.ContentType != "image/png" && welcome.SecondUpload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("SecondUpload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (welcome.SecondUpload.Length > 1048576)
                {
                    ModelState.AddModelError("SecondUpload", "Fayl ölcüsu maximum 1MB ola bilər");
                }
            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(welcome.Upload);
                welcome.Photo = fileName;

                var secondFileName = _fileManager.Upload(welcome.SecondUpload);
                welcome.SecondPhoto = secondFileName;

                _context.Add(welcome);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(welcome);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var welcome = await _context.WelcomeHospitals.FindAsync(id);
            if (welcome == null)
            {
                return NotFound();
            }
            return View(welcome);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Heading,Upload,Photo,Id,Content,SecondUpload,SecondPhoto")] WelcomeHospital welcome)
        {
            if (id != welcome.Id)
            {
                return NotFound();
            }

            if (welcome.SecondUpload == null)
            {
                ModelState.AddModelError("SecondUpload", "Şəkil məcburidir");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if(welcome.Upload != null)
                    {
                        if (welcome.Upload.ContentType != "image/jpeg" && welcome.Upload.ContentType != "image/png" && welcome.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(welcome);
                        }

                        if (welcome.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(welcome);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", welcome.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(welcome.Upload, "wwwroot/uploads");
                        welcome.Photo = fileName;
                    }
                    
                    if(welcome.SecondUpload != null)
                    {
                        if (welcome.SecondUpload.ContentType != "image/jpeg" && welcome.SecondUpload.ContentType != "image/png" && welcome.SecondUpload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("SecondUpload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(welcome);
                        }

                        if (welcome.SecondUpload.Length > 1048576)
                        {
                            ModelState.AddModelError("SecondUpload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(welcome);
                        }

                        var secondOldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", welcome.SecondPhoto);
                        _fileManager.Delete(secondOldFile);

                        var secondFileName = _fileManager.Upload(welcome.SecondUpload, "wwwroot/uploads");
                        welcome.SecondPhoto = secondFileName;
                    }

                     _context.Update(welcome);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WelcomeHospitalExsist(welcome.Id))
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
            return View(welcome);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var welcome = await _context.WelcomeHospitals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (welcome == null)
            {
                return NotFound();
            }

            return View(welcome);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var welcome = await _context.WelcomeHospitals.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", welcome.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");

                var secondOldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", welcome.SecondPhoto);
                _fileManager.Delete(secondOldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.WelcomeHospitals.Remove(welcome);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.WelcomeHospitals.Remove(welcome);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WelcomeHospitalExsist(int id)
        {
            return _context.WelcomeHospitals.Any(e => e.Id == id);
        }
    }
}
