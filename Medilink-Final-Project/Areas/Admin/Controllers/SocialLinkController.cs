using Medilink_Final_Project.Data;
using Medilink_Final_Project.Filter;
using Medilink_Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medilink_Final_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialLinkController : Controller
    {
        private readonly AplicationDbContext _context;
        private readonly IFileManager _fileManager;

        public SocialLinkController(AplicationDbContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.SocialLinks.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialLink = await _context.SocialLinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialLink == null)
            {
                return NotFound();
            }

            return View(socialLink);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Facebook,Twitter,Linkedin,Yotube")] SocialLink socialLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(socialLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(socialLink);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialLink = await _context.SocialLinks.FindAsync(id);
            if (socialLink == null)
            {
                return NotFound();
            }
            return View(socialLink);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Facebook,Twitter,Linkedin,Yotube")] SocialLink socialLink)
        {
            if (id != socialLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialLinkExists(socialLink.Id))
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
            return View(socialLink);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialLink = await _context.SocialLinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (socialLink == null)
            {
                return NotFound();
            }

            return View(socialLink);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socialLink = await _context.SocialLinks.FindAsync(id);
            _context.SocialLinks.Remove(socialLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialLinkExists(int id)
        {
            return _context.SocialLinks.Any(e => e.Id == id);
        }
    }
}
