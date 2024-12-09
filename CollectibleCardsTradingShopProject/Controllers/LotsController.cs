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
using System.Drawing.Printing;

namespace CollectibleCardsTradingShopProject.Controllers
{
    public class LotsController : Controller
    {
        private readonly int pageSize = 50;
        private readonly ApplicationDbContext _context;

        public LotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lots
        public async Task<IActionResult> Index(string franchise, string cardName, string rarity, int page = 1)
        {
            const int pageSize = 10; // Количество записей на странице
            page = page < 1 ? 1 : page; // Убедимся, что страница не меньше 1

            var lotsQuery = _context.Lots.AsQueryable();

            // Фильтрация по франшизе
            if (!string.IsNullOrEmpty(franchise))
            {
                lotsQuery = lotsQuery.Where(l => l.CardInLot.Any(cl => cl.Card.Franchise.Name.Contains(franchise)));
            }

            // Фильтрация по названию карты
            if (!string.IsNullOrEmpty(cardName))
            {
                lotsQuery = lotsQuery.Where(l => l.CardInLot.Any(cl => cl.Card.Name.Contains(cardName)));
            }

            // Фильтрация по редкости
            if (!string.IsNullOrEmpty(rarity))
            {
                lotsQuery = lotsQuery.Where(l => l.CardInLot.Any(cl => cl.Card.Rarity.Name.Contains(rarity)));
            }

            // Подсчет общего количества записей
            var count = await lotsQuery.CountAsync();

            // Применение пагинации
            var lots = await lotsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LotViewModel
                {
                    LotId = l.Id,
                    OpenedByUserName = l.UsersLot
                        .Where(ul => !ul.DidCloseTheLot)
                        .Select(ul => ul.User.UserName)
                        .FirstOrDefault(),
                    CardsSummary = string.Join(", ", l.CardInLot.Select(cl => cl.Card.Name).Take(5)) +
                                   (l.CardInLot.Count() > 5 ? "..." : ""),
                    ClosedByUserName = l.UsersLot
                        .Where(ul => ul.DidCloseTheLot)
                        .Select(ul => ul.User.UserName)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // Создание ViewModel
            var viewModel = new LotFilterViewModel
            {
                Franchise = franchise,
                CardName = cardName,
                Rarity = rarity,
                Lots = lots,
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return View(viewModel);
        }

        // GET: Lots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var lot = _context.Lots
                .Where(l => l.Id == id)
                .Select(l => new LotViewModel
                {
                    OpenedByUserName = l.UsersLot
                        .Where(ul => !ul.DidCloseTheLot)
                        .Select(ul => ul.User.UserName)
                        .FirstOrDefault(),
                    OpenedByEmail = l.UsersLot
                        .Where(ul => !ul.DidCloseTheLot)
                        .Select(ul => ul.User.Email)
                        .FirstOrDefault(),
                    Cards = l.CardInLot.Select(cl => new LotViewModel.CardInfo
                    {
                        Name = cl.Card.Name,
                        Image = cl.Card.Image,
                        Franchise = cl.Card.Franchise.Name,
                        Rarity = cl.Card.Rarity.Name,
                        Status = cl.LotCardStatus.Status
                    }).ToList(),
                    ClosedByUserName = l.UsersLot
                        .Where(ul => ul.DidCloseTheLot)
                        .Select(ul => ul.User.UserName)
                        .FirstOrDefault(),
                    ClosedByEmail = l.UsersLot
                        .Where(ul => ul.DidCloseTheLot)
                        .Select(ul => ul.User.Email)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // GET: Lots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Lot lot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lot);
        }

        // GET: Lots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lots.FindAsync(id);
            if (lot == null)
            {
                return NotFound();
            }
            return View(lot);
        }

        // POST: Lots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Lot lot)
        {
            if (id != lot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LotExists(lot.Id))
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
            return View(lot);
        }

        // GET: Lots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lot = await _context.Lots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lot == null)
            {
                return NotFound();
            }

            return View(lot);
        }

        // POST: Lots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lot = await _context.Lots.FindAsync(id);
            if (lot != null)
            {
                _context.Lots.Remove(lot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LotExists(int id)
        {
            return _context.Lots.Any(e => e.Id == id);
        }
    }
}
