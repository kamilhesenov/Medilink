using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public DepartmentController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Upload,Photo,Id,Content,Advantage,Text")] Department department)
        {
            if (department.Upload == null)
            {
                ModelState.AddModelError("Upload", "Şəkil məcburidir");
            }
            else
            {
                if (department.Upload.ContentType != "image/jpeg" && department.Upload.ContentType != "image/png" && department.Upload.ContentType != "image/gif")
                {
                    ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                }

                if (department.Upload.Length > 1048576)
                {
                    ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                }

            }
            if (ModelState.IsValid)
            {
                var fileName = _fileManager.Upload(department.Upload);
                department.Photo = fileName;

                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Upload,Photo,Id,Content,Advantage,Text")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    if (department.Upload != null)
                    {
                        if (department.Upload.ContentType != "image/jpeg" && department.Upload.ContentType != "image/png" && department.Upload.ContentType != "image/gif")
                        {
                            ModelState.AddModelError("Upload", "Siz yalnız png,jpg və ya gif faylı yükləyə bilərsiniz");
                            return View(department);
                        }

                        if (department.Upload.Length > 1048576)
                        {
                            ModelState.AddModelError("Upload", "Fayl ölcüsu maximum 1MB ola bilər");
                            return View(department);
                        }

                        var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", department.Photo);
                        _fileManager.Delete(oldFile);

                        var fileName = _fileManager.Upload(department.Upload, "wwwroot/uploads");
                        department.Photo = fileName;

                    }

                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExsist(department.Id))
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
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            try
            {
                var oldFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", department.Photo);
                _fileManager.Delete(oldFile, "wwwroot/uploads");
            }
            catch (FileNotFoundException)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExsist(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
