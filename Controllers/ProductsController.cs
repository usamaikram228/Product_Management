using CRUD.Models;
using CRUD.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRUD.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _repository;
        private readonly UserManager<IdentityUser> _userManager;
        private const int PageSize = 8; 

        public ProductsController(ApplicationDbContext context,IProductRepository repository,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _repository = repository;
            _userManager = userManager;
        }
        [Authorize(Roles = "User")]      
        
        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                IQueryable<Product> products = _context.Products.Where(p => p.UserId == user.Id);
                int totalCount = await products.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
                var paginatedProducts = await products.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

                ViewData["CurrentPage"] = page;
                ViewData["TotalPages"] = totalPages;

                return View(paginatedProducts);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        public async Task<IActionResult> Searching(string searchString)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var products = await _context.Products.Where(p => p.UserId == user.Id && (p.Name.Contains(searchString) || p.Category.Contains(searchString))).ToListAsync();
                return View("Index", products);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        public byte[] ConvertImageToByteArray(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpPost]
        public IActionResult AddedSuccessfully(Product product, IFormFile imageData)
        {
            try
            {
                if (imageData != null && imageData.Length > 0)
                {
                    product.ImageData = ConvertImageToByteArray(imageData);
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.UserId = userId;
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");
            }
            catch (Exception)
            {
                return View("AddPoduct");
            }
        }
        public IActionResult UpdateProduct(int id)
        {
           var product = _repository.getProductByid(id); ;
            return View(product);
        }

        public IActionResult UpdatedProduct(Product product, IFormFile NewImage)
        {
            var productToBeUpdated = _repository.getProductByid(product.Id);

           if (productToBeUpdated != null)
            {
                productToBeUpdated.Name = product.Name;
                productToBeUpdated.Description = product.Description;
                productToBeUpdated.Price = product.Price;
                productToBeUpdated.Category = product.Category;
                if (NewImage != null && NewImage.Length > 0)
                {
                    productToBeUpdated.ImageData = ConvertImageToByteArray(NewImage);
                }
                _context.Entry(productToBeUpdated).State = EntityState.Modified;
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index", "Home");
        }

    public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            // Step 2: Check if the product exists
            if (product == null)
            {
                return NotFound(); 
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");

        }

    }
}
