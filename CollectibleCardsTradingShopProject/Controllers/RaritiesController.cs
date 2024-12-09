using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollectibleCardsTradingShopProject.Data;
using CollectibleCardsTradingShopProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace CollectibleCardsTradingShopProject.Controllers
{
    public class RaritiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaritiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rarities
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rarities.ToListAsync());
        }

        // GET: Rarities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rarity = await _context.Rarities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rarity == null)
            {
                return NotFound();
            }

            return View(rarity);
        }

        // GET: Rarities/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rarities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Rarity rarity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rarity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rarity);
        }

        // GET: Rarities/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rarity = await _context.Rarities.FindAsync(id);
            if (rarity == null)
            {
                return NotFound();
            }
            return View(rarity);
        }

        // POST: Rarities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Rarity rarity)
        {
            if (id != rarity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rarity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RarityExists(rarity.Id))
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
            return View(rarity);
        }

        // GET: Rarities/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rarity = await _context.Rarities
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rarity == null)
            {
                return NotFound();
            }

            return View(rarity);
        }

        // POST: Rarities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rarity = await _context.Rarities.FindAsync(id);
            if (rarity != null)
            {
                _context.Rarities.Remove(rarity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RarityExists(int id)
        {
            return _context.Rarities.Any(e => e.Id == id);
        }
    }
}
