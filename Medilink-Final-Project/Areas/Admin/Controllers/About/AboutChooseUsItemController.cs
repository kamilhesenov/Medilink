using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models.About;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers.About
{
    [Area("Admin")]
    public class AboutChooseUsItemController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public AboutChooseUsItemController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.AboutChooseUsItems.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutChooseUsItem = await _context.AboutChooseUsItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutChooseUsItem == null)
            {
                return NotFound();
            }

            return View(aboutChooseUsItem);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content")] AboutChooseUsItem aboutChooseUsItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutChooseUsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutChooseUsItem);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutChooseUsItem = await _context.AboutChooseUsItems.FindAsync(id);
            if (aboutChooseUsItem == null)
            {
                return NotFound();
            }
            return View(aboutChooseUsItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] AboutChooseUsItem aboutChooseUsItem)
        {
            if (id != aboutChooseUsItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutChooseUsItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutChooseUsItemExists(aboutChooseUsItem.Id))
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
            return View(aboutChooseUsItem);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutChooseUsItem = await _context.AboutChooseUsItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutChooseUsItem == null)
            {
                return NotFound();
            }

            return View(aboutChooseUsItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aboutChooseUsItem = await _context.AboutChooseUsItems.FindAsync(id);
            _context.AboutChooseUsItems.Remove(aboutChooseUsItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutChooseUsItemExists(int id)
        {
            return _context.AboutChooseUsItems.Any(e => e.Id == id);
        }
    }
}
