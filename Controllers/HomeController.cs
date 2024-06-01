using CRUD.Models;
using CRUD.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _repository;
        private const int PageSize = 8; 

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context,IProductRepository productRepository)
        {
            _logger = logger;
            _context = context;
            _repository = productRepository;
        }

        public IActionResult Index(int page = 1)
        {
            IQueryable<Product> products = _context.Products;
            int totalCount = products.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
            var paginatedProducts = products.Skip((page - 1) * PageSize).Take(PageSize).ToList();

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("login", "login");
            }

            return View(paginatedProducts);
        }
        public async Task<IActionResult> Searching(string searchString)
        {
            IQueryable<Product> products = await _repository.GetProductsAsync(searchString);
            return View("Index", products.ToList());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
