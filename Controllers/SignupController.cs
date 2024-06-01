using CRUD.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    public class SignupController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SignupController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
      
        public IActionResult Signup()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {   
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                    return View("Signup", model);
                   
                }
                IdentityUser user = new()
                {
                    UserName = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync("user");
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                    }
                    await _userManager.AddToRoleAsync(user, "user");
                    return RedirectToAction("Login", "Login");
                }
            }

            return RedirectToAction("Signup", "Signup");
        }

    }
}

