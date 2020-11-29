using System.Collections.Generic;
using System.Linq;
using Kata.Checkout.Models;

namespace Kata.Checkout.Products
{
    public class ProductDemoService : IProductService
    {
        private List<Item> _items = new List<Item>();

        public ProductDemoService()
        {
            _items.Add(new Item(){Sku = "A",UnitPrice = 10});
            _items.Add(new Item() { Sku = "B", UnitPrice = 15 });
            _items.Add(new Item() { Sku = "C", UnitPrice = 40 });
            _items.Add(new Item() { Sku = "D", UnitPrice = 55 });
        }

        public Item GetBySku(string sku)
        {
            return _items.FirstOrDefault(a=> string.CompareOrdinal(a.Sku, sku) ==0);
        }
    }
}
