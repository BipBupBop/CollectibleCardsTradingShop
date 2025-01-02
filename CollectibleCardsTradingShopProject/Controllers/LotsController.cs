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
using System.Security.Claims;
using CollectibleCardsTradingShopProject.ViewModels.Users;
using CollectibleCardsTradingShopProject.ViewModels.CreateLot;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Index(string franchise, string cardName, string rarity, bool seekOpened, int page = 1)
        {
            const int pageSize = 10;
            page = page < 1 ? 1 : page; 

            var lotsQuery = _context.Lots.AsQueryable();

            if (seekOpened)
            {
                lotsQuery = _context.UserLots
                    .Where(ul => ul.DidCloseTheLot == false)
                    .Select(ul => ul.Lot)
                    .AsQueryable()
                    .Where(lot => !_context.UserLots
                        .Any(ul => ul.LotId == lot.Id && ul.DidCloseTheLot == true));
            }

            if (!string.IsNullOrEmpty(franchise))
            {
                lotsQuery = lotsQuery.Where(l => l.CardInLot.Any(cl => cl.Card.Franchise.Name.Contains(franchise)));
            }

            if (!string.IsNullOrEmpty(cardName))
            {
                lotsQuery = lotsQuery.Where(l => l.CardInLot.Any(cl => cl.Card.Name.Contains(cardName)));
            }

            if (!string.IsNullOrEmpty(rarity))
            {
                lotsQuery = lotsQuery.Where(l => l.CardInLot.Any(cl => cl.Card.Rarity.Name.Contains(rarity)));
            }            

            var count = await lotsQuery.CountAsync();

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

            var viewModel = new LotFilterViewModel
            {
                Franchise = franchise,
                CardName = cardName,
                Rarity = rarity,
                Lots = lots,
                SeekOpened = seekOpened,
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MyLots(int page = 1)
        {
            const int pageSize = 10;
            page = page < 1 ? 1 : page;

            var lotsQuery = _context.Lots.AsQueryable();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized(); // Убедимся, что пользователь аутентифицирован
            }

            lotsQuery = _context.UserLots
                    .Where(ul => ul.UserId == currentUserId)
                    .Select(ul => ul.Lot)
                    .AsQueryable()
                    .Where(lot => !_context.UserLots
                        .Any(ul => ul.LotId == lot.Id && ul.DidCloseTheLot == true));

            var count = await lotsQuery.CountAsync();

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

            var viewModel = new LotFilterViewModel
            {
                Franchise = "",
                CardName = "",
                Rarity = "",
                Lots = lots,
                SeekOpened = false,
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return View("Index", viewModel);
        }

        // GET: Lots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var lot = _context.Lots
                .Where(l => l.Id == id)
                .Select(l => new LotViewModel
                {
                    LotId = l.Id,
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
                        Status = cl.LotCardStatus.Status,
                        Quantity = cl.Quantity
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

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            if(lot.OpenedByEmail != _context.Users.Where(u => u.Id == currentUserId).FirstOrDefault().Email)
                lot.CurrentUserWatchingIsNotOwner = true;

            return View(lot);
        }

        [HttpPost]
        public IActionResult ApplyByCurrentUser(int lotId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var lotCardsRequired = _context.CardInLots
                .Where(cl => cl.LotId == lotId)
                .Where(cl => cl.LotCardStatusId == 3) // 3 is "required"
                .Select(cl => cl.Card)
                .ToList();

            var currentUserCards = _context.UserCards
                .Where(uc => uc.UserId == currentUserId)
                .ToList();

            bool userHasRequiredCards = lotCardsRequired.All(card =>
                currentUserCards.Any(uc => uc.CardId == card.Id && uc.Quantity > 0));

            if (!userHasRequiredCards)
            {
                return BadRequest("You do not have all the required cards for this lot.");
            }

            var lotCardsToReceive = _context.CardInLots
                .Where(cl => cl.LotId == lotId)
                .Where(cl => cl.LotCardStatusId == 1) // 1 is "to receive"
                .Select(cl => cl.Card)
                .ToList();

            string ownerId = _context.UserLots
                .Where(ul => ul.LotId == lotId)
                .Select(ul => ul.UserId)
                .FirstOrDefault();

            if (ownerId == null)
            {
                return BadRequest("Lot owner not found.");
            }

            var ownerCards = _context.UserCards
                .Where(uc => uc.UserId == ownerId)
                .ToList();

            bool ownerHasRequiredCards = lotCardsToReceive.All(card =>
                ownerCards.Any(uc => uc.CardId == card.Id && uc.Quantity > 0));

            if (!ownerHasRequiredCards)
            {
                return BadRequest("The lot owner does not have all the required cards.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TransactCards(currentUserId, lotId);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return StatusCode(500, "An error occurred while processing the transaction.");
                }
            }

            return RedirectToAction("Index");
        }


        private void TransactCards(string currentUserId, int lotId)
        {
            var cardsToGive = _context.CardInLots
                .Include(cl => cl.Card.Franchise)
                .Include(cl => cl.Card.Rarity)
                .Where(cl => cl.LotId == lotId && cl.LotCardStatusId == 3)
                .Select(cl => cl.Card)
                .ToList();

            var cardsToRecieve = _context.CardInLots
                .Include(cl => cl.Card.Franchise)
                .Include(cl => cl.Card.Rarity)
                .Where(cl => cl.LotId == lotId && cl.LotCardStatusId == 1)
                .Select(cl => cl.Card)
                .ToList();

            string ownerId = _context.UserLots
                .Where(ul => ul.LotId == lotId)
                .Select(ul => ul.UserId)
                .FirstOrDefault();

            foreach (var card in cardsToGive)
            {
                var userCard = _context.UserCards
                    .FirstOrDefault(uc => uc.UserId == currentUserId && uc.CardId == card.Id);

                if (userCard != null)
                {
                    userCard.Quantity--;
                    if (userCard.Quantity <= 0)
                    {
                        _context.UserCards.Remove(userCard);
                    }
                }

                var ownerCard = _context.UserCards
                    .FirstOrDefault(uc => uc.UserId == ownerId && uc.CardId == card.Id);

                if (ownerCard != null)
                {
                    ownerCard.Quantity++;
                }
                else
                {
                    _context.UserCards.Add(new UserCard
                    {
                        UserId = ownerId,
                        CardId = card.Id,
                        Quantity = 1
                    });
                }
            }

            foreach (var card in cardsToRecieve)
            {
                var ownerCard = _context.UserCards
                    .FirstOrDefault(uc => uc.UserId == ownerId && uc.CardId == card.Id);

                if (ownerCard != null)
                {
                    ownerCard.Quantity--;
                    if (ownerCard.Quantity <= 0)
                    {
                        _context.UserCards.Remove(ownerCard);
                    }
                }

                var userCard = _context.UserCards
                    .FirstOrDefault(uc => uc.UserId == currentUserId && uc.CardId == card.Id);

                if (userCard != null)
                {
                    userCard.Quantity++;
                }
                else
                {
                    _context.UserCards.Add(new UserCard
                    {
                        UserId = currentUserId,
                        CardId = card.Id,
                        Quantity = 1
                    });
                }
            }

            _context.UserLots.Add(new UserLot()
            {
                UserId = currentUserId,
                DidCloseTheLot = true,
                LotId = lotId 
            });

            _context.SaveChanges();
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Lot lot)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.Add(lot);
                await _context.SaveChangesAsync();
                _context.UserLots.Add(new UserLot { UserId = currentUserId, LotId = lot.Id });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lot);
        }

        [HttpGet]
        public async Task<IActionResult> CreateLot(
    string userSearchQuery ="",
    string dbSearchQuery ="",
    int userPage = 1,
    int dbPage = 1,
    string offeredCardsData = null,
    string requiredCardsData = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }

            var userCardsQuery = _context.UserCards
                .Where(uc => uc.UserId == currentUserId)
                .Select(uc => new LotCardViewModel
                {
                    Id = uc.Card.Id,
                    Name = uc.Card.Name,
                    Image = uc.Card.Image,
                    Franchise = uc.Card.Franchise.Name,
                    Rarity = uc.Card.Rarity.Name,
                    Quantity = uc.Quantity
                });

            if (!string.IsNullOrWhiteSpace(userSearchQuery))
            {
                userCardsQuery = userCardsQuery.Where(c => c.Name.Contains(userSearchQuery));
            }

            var userCards = await PaginatedList<LotCardViewModel>.CreateAsync(userCardsQuery, userPage, 5);

            var databaseCardsQuery = _context.Cards
                .Select(c => new LotCardViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image,
                    Franchise = c.Franchise.Name,
                    Rarity = c.Rarity.Name,
                    Quantity = 1
                });

            if (!string.IsNullOrWhiteSpace(dbSearchQuery))
            {
                databaseCardsQuery = databaseCardsQuery.Where(c => c.Name.Contains(dbSearchQuery));
            }

            var databaseCards = await PaginatedList<LotCardViewModel>.CreateAsync(databaseCardsQuery, dbPage, 5);

            var offeredCards = string.IsNullOrEmpty(offeredCardsData)
                ? new List<LotCardViewModel>()
                : JsonConvert.DeserializeObject<List<LotCardViewModel>>(offeredCardsData);

            var requiredCards = string.IsNullOrEmpty(requiredCardsData)
                ? new List<LotCardViewModel>()
                : JsonConvert.DeserializeObject<List<LotCardViewModel>>(requiredCardsData);

            var viewModel = new LotCreationViewModel
            {
                UserCards = userCards,
                DatabaseCards = databaseCards,
                OfferedCards = offeredCards,
                RequiredCards = requiredCards,
                UserSearchQuery = userSearchQuery,
                DatabaseSearchQuery = dbSearchQuery
            };

            return View(viewModel);
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
