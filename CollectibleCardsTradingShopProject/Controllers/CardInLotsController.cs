using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollectibleCardsTradingShopProject.Data;
using CollectibleCardsTradingShopProject.Models;
using System.Security.Claims;

namespace CollectibleCardsTradingShopProject.Controllers
{
    public class CardInLotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CardInLotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CardInLots
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized(); // Убедимся, что пользователь аутентифицирован
            }

            // Получаем список Id лотов, которые связаны с текущим пользователем
            var userLotIds = _context.UserLots
                .Where(ul => ul.UserId == currentUserId)
                .Select(ul => ul.LotId) // или просто .Select(ul => ul.Id), если вам нужно Id записи в таблице UserLots
                .ToList();

            // Фильтруем карты по лотам, проверяя, что лот находится в списке лотов пользователя
            var applicationDbContext = _context.CardInLots
                .Include(c => c.Card)
                .Include(c => c.Lot)
                .Include(c => c.LotCardStatus)
                .Where(cl => userLotIds.Contains(cl.Lot.Id)); // Оставляем только те карты, чьи лоты принадлежат текущему пользователю
            
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CardInLots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardInLot = await _context.CardInLots
                .Include(c => c.Card)
                .Include(c => c.Lot)
                .Include(c => c.LotCardStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardInLot == null)
            {
                return NotFound();
            }

            return View(cardInLot);
        }

        // GET: CardInLots/Create
        public IActionResult Create()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized(); // Убедимся, что пользователь аутентифицирован
            }

            // Получаем список Id лотов, которые связаны с текущим пользователем
            var userLotsIds = _context.UserLots
                .Where(ul => ul.UserId == currentUserId)
                .Select(ul => ul.Lot.Id) // или просто .Select(ul => ul.Id), если вам нужно Id записи в таблице UserLots
                .ToList();

            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Name");
            ViewData["LotId"] = new SelectList(_context.Lots.Where(l => userLotsIds.Contains(l.Id)), "Id", "Id");
            ViewData["LotCardStatusId"] = new SelectList(_context.LotCardStatuses, "Id", "Status");

            return View();
        }

        // POST: CardInLots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,CardId,LotCardStatusId,LotId")] CardInLot cardInLot)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cardInLot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", cardInLot.CardId);
            ViewData["LotId"] = new SelectList(_context.Lots, "Id", "Id", cardInLot.LotId);
            ViewData["LotCardStatusId"] = new SelectList(_context.LotCardStatuses, "Id", "Id", cardInLot.LotCardStatusId);
            return View(cardInLot);
        }

        // GET: CardInLots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardInLot = await _context.CardInLots.FindAsync(id);
            if (cardInLot == null)
            {
                return NotFound();
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized(); // Убедимся, что пользователь аутентифицирован
            }

            // Получаем список Id лотов, которые связаны с текущим пользователем
            var userLotsIds = _context.UserLots
                .Where(ul => ul.UserId == currentUserId)
                .Select(ul => ul.Lot.Id) // или просто .Select(ul => ul.Id), если вам нужно Id записи в таблице UserLots
                .ToList();

            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id");
            ViewData["LotId"] = new SelectList(_context.Lots.Where(l => userLotsIds.Contains(l.Id)), "Id", "Id");
            ViewData["LotCardStatusId"] = new SelectList(_context.LotCardStatuses, "Id", "Id", cardInLot.LotCardStatusId);
            return View(cardInLot);
        }

        // POST: CardInLots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,CardId,LotCardStatusId,LotId")] CardInLot cardInLot)
        {
            if (id != cardInLot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cardInLot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardInLotExists(cardInLot.Id))
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
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Id", cardInLot.CardId);
            ViewData["LotId"] = new SelectList(_context.Lots, "Id", "Id", cardInLot.LotId);
            ViewData["LotCardStatusId"] = new SelectList(_context.LotCardStatuses, "Id", "Id", cardInLot.LotCardStatusId);
            return View(cardInLot);
        }

        // GET: CardInLots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cardInLot = await _context.CardInLots
                .Include(c => c.Card)
                .Include(c => c.Lot)
                .Include(c => c.LotCardStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardInLot == null)
            {
                return NotFound();
            }

            return View(cardInLot);
        }

        // POST: CardInLots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cardInLot = await _context.CardInLots.FindAsync(id);
            if (cardInLot != null)
            {
                _context.CardInLots.Remove(cardInLot);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardInLotExists(int id)
        {
            return _context.CardInLots.Any(e => e.Id == id);
        }
    }
}
