using CRUD.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _repository;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IProductRepository repository)
        {
            _userManager = userManager;
            _context = context;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users()
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync("user");
                return View(usersInRole);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching users.";
                return View("Error");
            }
        }

        public async Task<IActionResult> Admins()
        {
            try
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync("admin");
                return View(usersInRole);
            }
            catch (Exception)
            {    ViewBag.ErrorMessage = "An error occurred while fetching admins.";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRoleToAdmin(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    ViewBag.ErrorMessage = "User ID is required.";
                    return View("Error");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with ID '{userId}' not found.";
                    return View("Error");
                }

                var isInAdminRole = await _userManager.IsInRoleAsync(user, "admin");
                if (isInAdminRole)
                {
                    ViewBag.ErrorMessage = $"User '{user.UserName}' is already an admin.";
                    return View("Error");
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, "admin");
                if (!addRoleResult.Succeeded)
                {
                    ViewBag.ErrorMessage = $"Failed to assign admin role to user '{user.UserName}'.";
                    return View("Error");
                }

                return RedirectToAction("Users");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while processing the request.";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdminRole(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    ViewBag.ErrorMessage = "User ID is required.";
                    return View("Error");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with ID '{userId}' not found.";
                    return View("Error");
                }

                if (user.UserName == "admin@gmail.com")
                {
                    ViewBag.ErrorMessage = "Cannot remove admin role from the default admin user.";
                    return View("Error");
                }

                var isInRole = await _userManager.IsInRoleAsync(user, "admin");
                if (!isInRole)
                {
                    ViewBag.ErrorMessage = $"User '{user.UserName}' is not an admin.";
                    return View("Error");
                }

                var result = await _userManager.RemoveFromRoleAsync(user, "admin");
                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage = $"Failed to remove admin role from user '{user.UserName}'.";
                    return View("Error");
                }

                return RedirectToAction("Admins");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while processing the request.";
                return View("Error");
            }
        }

        public async Task<IActionResult> DeleteUserAndProducts(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var userProducts = _context.Products.Where(p => p.UserId == userId).ToList();
                foreach (var product in userProducts)
                {
                    _context.Products.Remove(product);
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ViewBag.ErrorMessage = "Failed to delete user.";
                    return View("Error");
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "An error occurred while processing the request.";
                return View("Error");
            }
        }

        public async Task<IActionResult> UserProducts(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest();
                }
                var products = await _repository.GetProductsByUserIdAsync(id);
                return View("userProducts", products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while fetching user products.";
                return View("Error");
            }
        }
    }
}
