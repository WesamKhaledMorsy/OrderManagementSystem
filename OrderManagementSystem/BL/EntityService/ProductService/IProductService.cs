using OrderManagementSystem.DL.Entities;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.BL.EntityService.ProductService
{
    public interface IProductService
    {
        Product CreateNewProduct(ProductModel product);
        Product UpdateProduct(ProductModel product);
        IQueryable<Product> GetAllProducts();
        Product GetProductById(int productId);

    }
}
