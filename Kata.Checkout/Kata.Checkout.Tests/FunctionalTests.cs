using System.Linq;
using FluentAssertions;
using Kata.Checkout.Pricing;
using Kata.Checkout.Products;
using Kata.Checkout.Promotions;
using NUnit.Framework;

namespace Kata.Checkout.Tests
{
    public class FunctionalTests
    {
        private IProductService _productService;
        private IShoppingBasket _shoppingBasket;
        private IPricingEngine _pricingEngine;
        private IPromotionProvider _promotionProvider;

        [SetUp]
        public void SetUp()
        {
            _productService = new ProductDemoService();
            _promotionProvider = new PromotionProvider();
            _pricingEngine = new PricingEngine(_promotionProvider);
            _shoppingBasket = new ShoppingBasket(_pricingEngine);
        }


        [Test(Description = "Given I have selected to add an item to the basket Then the item should be added to the basket")]
        public void Test1()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 7;
            _shoppingBasket.AddToBasket(itemA, quantity);

            var result = _shoppingBasket.GetBasket().ToList();

            result.Count().Should().Be(1);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);
        }

        [Test(Description = "Given items have been added to the basket Then the total cost of the basket should be calculated")]
        public void Test2()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 2;

            var itemB = _productService.GetBySku("C");
            int quantityB = 2;

            _shoppingBasket.AddToBasket(itemA, quantity);
            _shoppingBasket.AddToBasket(itemB, quantityB);

            var result = _shoppingBasket.GetBasket().ToList();
            result.Count().Should().Be(2);

            var total = _shoppingBasket.TotalPrice();
            total.Should().Be(100m);
        }

        [Test(Description = "Given I have added a multiple of 3 lots of item ‘B’ to the basket Then a promotion of ‘3 for 40’ should be applied to every multiple of 3")]
        public void Test3()
        {
            var itemA = _productService.GetBySku("B");
            int quantity = 3;
            _shoppingBasket.AddToBasket(itemA, quantity);

            var result = _shoppingBasket.GetBasket().ToList();

            result.Count().Should().Be(1);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);

            var total = _shoppingBasket.TotalPrice();
            total.Should().Be(40m);
        }

        [Test(Description = "Given I have added a multiple of 2 lots of item ‘D’ to the basket Then a promotion of ‘25% off’ should be applied to every multiple of 2")]
        public void Test4()
        {
            var itemA = _productService.GetBySku("D");
            int quantity = 2;
            _shoppingBasket.AddToBasket(itemA, quantity);

            var result = _shoppingBasket.GetBasket().ToList();

            result.Count().Should().Be(1);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);

            var total = _shoppingBasket.TotalPrice();
            total.Should().Be(82.5m); // Pre promo total 110 - 25% discount (27.5) - 110-27.5 = 82.5
        }
    }
}
