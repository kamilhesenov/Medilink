using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers.Home
{
    [Area("Admin")]
    public class HomeSpecialitySuccessController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public HomeSpecialitySuccessController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.homeSpecialitySucces.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeSuccess = await _context.homeSpecialitySucces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeSuccess == null)
            {
                return NotFound();
            }

            return View(homeSuccess);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Text")] HomeSpecialitySucces homeSuccess)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homeSuccess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homeSuccess);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeSuccess = await _context.homeSpecialitySucces.FindAsync(id);
            if (homeSuccess == null)
            {
                return NotFound();
            }
            return View(homeSuccess);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Text")] HomeSpecialitySucces homeSuccess)
        {
            if (id != homeSuccess.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeSuccess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeSuccessExists(homeSuccess.Id))
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
            return View(homeSuccess);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeSuccess = await _context.homeSpecialitySucces
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeSuccess == null)
            {
                return NotFound();
            }

            return View(homeSuccess);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeSuccess = await _context.homeSpecialitySucces.FindAsync(id);
            _context.homeSpecialitySucces.Remove(homeSuccess);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeSuccessExists(int id)
        {
            return _context.homeSpecialitySucces.Any(e => e.Id == id);
        }
    }
}
