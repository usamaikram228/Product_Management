using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Product getProductByid(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
        public async Task<List<Product>> GetProductsByUserIdAsync(string userId)
        {
            return await _context.Products
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }
        public async Task<IQueryable<Product>> GetProductsAsync(string searchString = null)
        {
            IQueryable<Product> products = _context.Products;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString) || p.Category.Contains(searchString));
            }

            return products;
        }
    }
}
