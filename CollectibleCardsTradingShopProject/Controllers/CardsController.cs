using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollectibleCardsTradingShopProject.Data;
using CollectibleCardsTradingShopProject.Models;
using CollectibleCardsTradingShopProject.ViewModels;
using Microsoft.Data.SqlClient;
using System.Drawing.Printing;

namespace CollectibleCardsTradingShopProject.Controllers
{
    public class CardsController : Controller
    {
        private readonly int pageSize = 50;

        private readonly ApplicationDbContext _context;

        public CardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cards
        public async Task<IActionResult> Index(string Franchise = "", string Name = "", SortState sortOrder = SortState.No, int page = 1)
        {
            IQueryable<Card> cardsContext = _context.Cards
                .Include(c => c.Franchise)
                .Include(c => c.Rarity);

            cardsContext = SortSearch(cardsContext, sortOrder, Franchise ?? "", Name ?? "");

            var count = await cardsContext.CountAsync();

            var cards = await cardsContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            CardsViewModel cardsViewModel = new()
            {
                Cards = cards,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                Franchise = Franchise
            };

            return View(cardsViewModel);
        }
        private IQueryable<Card> SortSearch(IQueryable<Card> cardsContext, SortState sortOrder, string FranchiseForFilter, string Name)
        {
            switch (sortOrder)
            {
                case SortState.FranchiseAsc:
                    cardsContext = cardsContext.OrderBy(s => s.Franchise.Name);
                    break;
                case SortState.FranchiseDesc:
                    cardsContext = cardsContext.OrderByDescending(s => s.Franchise.Name);
                    break;
                case SortState.RarityAsc:
                    cardsContext = cardsContext.OrderBy(s => s.Rarity.Id);
                    break;
                case SortState.RarityDesc:
                    cardsContext = cardsContext.OrderByDescending(s => s.Rarity.Id);
                    break;
                case SortState.NameAsc:
                    cardsContext = cardsContext.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    cardsContext = cardsContext.OrderByDescending(s => s.Name);
                    break;
            }
            if (FranchiseForFilter != "")
            {
                //cardsContext = cardsContext.Where(o => o.Franchise.Name.Contains(FranchiseForFilter ?? ""));
                cardsContext = cardsContext.Where(c => c.Franchise.Name == FranchiseForFilter);
            }
            if (Name != "")
            {
                cardsContext = cardsContext.Where(c => c.Name == Name);
            }
            return cardsContext;
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include(c => c.Franchise)
                .Include(c => c.Rarity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/Create
        public IActionResult Create()
        {
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "Id", "Id");
            ViewData["RarityId"] = new SelectList(_context.Rarities, "Id", "Id");
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,FranchiseId,RarityId")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "Id", "Id", card.FranchiseId);
            ViewData["RarityId"] = new SelectList(_context.Rarities, "Id", "Id", card.RarityId);
            return View(card);
        }

        // GET: Cards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "Id", "Id", card.FranchiseId);
            ViewData["RarityId"] = new SelectList(_context.Rarities, "Id", "Id", card.RarityId);
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,FranchiseId,RarityId")] Card card)
        {
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
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
            ViewData["FranchiseId"] = new SelectList(_context.Franchises, "Id", "Id", card.FranchiseId);
            ViewData["RarityId"] = new SelectList(_context.Rarities, "Id", "Id", card.RarityId);
            return View(card);
        }

        // GET: Cards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include(c => c.Franchise)
                .Include(c => c.Rarity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
