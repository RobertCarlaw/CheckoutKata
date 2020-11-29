using Kata.Checkout.Models;

namespace Kata.Checkout.Products
{
    public interface IProductService
    {
        Item GetBySku(string sku);
    }
}
