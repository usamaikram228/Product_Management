using CRUD.Models;

namespace CRUD.Repository
{
    public interface IProductRepository
    {
        Product getProductByid(int id);
        Task<List<Product>> GetProductsByUserIdAsync(string userId);

        Task<IQueryable<Product>> GetProductsAsync(string searchString = null);

    }
}
