using CollectibleCardsTradingShopProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using CollectibleCardsTradingShopProject.ViewModels.Users;
using Microsoft.EntityFrameworkCore;
using CollectibleCardsTradingShopProject.Data;
using CollectibleCardsTradingShopProject.ViewModels;

namespace CollectibleCardsTradingShopProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsersController(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager, ApplicationDbContext context)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.OrderBy(user => user.Id).ToList();

            List<UserViewModel> userViewModel = new List<UserViewModel>();

            var rolesList = _roleManager.Roles.ToList();
            var userRoles = _context.UserRoles.ToList(); 

            foreach (var user in users)
            {
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                var roleName = userRole != null
                    ? rolesList.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name
                    : "No Role";

                userViewModel.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleName = roleName
                });
            }

            return View(userViewModel);
        }

        // GET: Cards/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var userRawCards = _context.Cards
                .Where(c => _context.UserCards.Any(uc => uc.CardId == c.Id && uc.UserId == user.Id))
                .Include(c => c.Franchise)
                .Include(c => c.Rarity)
                .ToList();

            var userCards = new List<UserCardViewModel>();

            foreach (var card in userRawCards)
            {
                int quantity = _context.UserCards
                    .FirstOrDefault(uc => uc.CardId == card.Id && uc.UserId == id)
                    .Quantity;
                userCards.Add(new UserCardViewModel() { Card = card, Quantity = quantity });
            }
                

            var userRoleId = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .FirstOrDefault();

            var role = _roleManager.Roles
                .FirstOrDefault(r => r.Id == userRoleId)?.Name;


            var userDetailsView = new UserDetailsViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                RoleName = role,
                Email = user.Email,
                Cards = userCards
            };

            return View(userDetailsView);
        }
        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();
            CreateUserViewModel user = new();

            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");

            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    Email = model.Email,
                    UserName = model.UserName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                var role = model.UserRole;
                if (role.Length > 0)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            string userRole = "";
            if (userRoles.Count > 0)
            {
                userRole = userRoles[0] ?? "";
            }

            EditUserViewModel model = new()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                UserRole = userRole
            };
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name", model.UserRole);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var oldRoles = await _userManager.GetRolesAsync(user);

                    if (oldRoles.Count > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(user, oldRoles);

                    }
                    var newRole = model.UserRole;
                    if (newRole.Length > 0)
                    {
                        await _userManager.AddToRoleAsync(user, newRole);
                    }
                    user.Email = model.Email;
                    user.UserName = model.UserName;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> AddCardToUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ViewData["UserId"] = id; 
            ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCardToUser(CardInUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CardId"] = new SelectList(_context.Cards, "Id", "Name", model.CardId);
                return View(model);
            }

            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var card = new UserCard
            {
                UserId = model.UserId,
                CardId = model.CardId,
                Quantity = model.Quantity
            };

            _context.UserCards.Add(card);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
