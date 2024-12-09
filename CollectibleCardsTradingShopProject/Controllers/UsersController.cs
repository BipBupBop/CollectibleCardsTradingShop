using CollectibleCardsTradingShopProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using CollectibleCardsTradingShopProject.ViewModels.Users;

namespace CollectibleCardsTradingShopProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager)
        {
            this._roleManager = _roleManager;
            this._userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.OrderBy(user => user.Id);

            List<UserViewModel> userViewModel = [];

            string urole = "";

            var rolesList = _roleManager.Roles.ToList();
            foreach (var user in users)
            {
                if (rolesList.Count > 0)
                {
                    urole = rolesList.FirstOrDefault().Name;
                }

                userViewModel.Add(
                    new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        RoleName = urole
                    });

            }

            return View(userViewModel);
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
    }
}
