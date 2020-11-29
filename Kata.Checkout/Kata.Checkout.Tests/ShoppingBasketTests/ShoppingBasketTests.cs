using System;
using System.Linq;
using FluentAssertions;
using Kata.Checkout.Models;
using Kata.Checkout.Products;
using NUnit.Framework;

namespace Kata.Checkout.Tests.ShoppingBasketTests
{
    public class ShoppingBasketTests
    {
        #region Setup

        private ProductDemoService _productService;

        [SetUp]
        public void SetUp()
        {
            _productService = new ProductDemoService();
        }

        #endregion

        #region Adding/Getting Basket

        [Test]
        public void GivenAddToBasketWhenCartItemsIsNullThenThrowException()
        {
            var basket = new ShoppingBasket();

            Assert.Throws<ArgumentNullException>(() => basket.AddToBasket(null,6));
        }

        [Test(Description = "Given AddToBasket When quantity is zero Then throw exception")]
        public void GivenAddToBasketWhenQuantityIsZeroThenThrowException()
        {
            var itemA = _productService.GetBySku("A");
            var basket = new ShoppingBasket();

            Assert.Throws<ArgumentOutOfRangeException>(() => basket.AddToBasket(itemA, 0));
        }

        [Test (Description = "Given I have added a item to the basket When requesting basket Then item is returned correctly")]
        public void GivenIHaveAddedAItemToTheBasketThenTheItemIsInTheBasket()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 7;

            var basket = new ShoppingBasket();
            basket.AddToBasket(itemA, quantity);

            var result = basket.GetBasket().ToList();

            result.Count().Should().Be(1);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);
            result[0].LineTotal.Should().Be(quantity * itemA.UnitPrice);
        }

        [Test (Description = "Given many items are added When requesting basket Then items are returned correctly")]
        public void GivenIAddManyItemsToABasketThenItemsAreReturnedCorrectly()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 2;

            var itemB = _productService.GetBySku("B");
            int quantity1 = 3;

            var basket = new ShoppingBasket();

            basket.AddToBasket(itemA, quantity);
            basket.AddToBasket(itemB, quantity1);

            var result = basket.GetBasket().ToList();

            result.Count().Should().Be(2);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);
            result[0].LineTotal.Should().Be(quantity * itemA.UnitPrice);
            result[1].Item.Should().BeEquivalentTo(itemB);
            result[1].Quantity.Should().Be(quantity1);
            result[1].LineTotal.Should().Be(quantity1 * itemB.UnitPrice);
        }

        #endregion

        #region GetTotal

        [Test(Description = "Given GetTotal When many items in basket Then calculate correctly")]
        public void GivenGetTotalWhenManyItemsInBAsketThenCalculateCorrectly()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 2;

            var itemB = _productService.GetBySku("B");
            int quantity1 = 3;

            var basket = new ShoppingBasket();

            basket.AddToBasket(itemA, quantity);
            basket.AddToBasket(itemB, quantity1);

            var result = basket.TotalPrice();

            var itemATotal = itemA.UnitPrice * quantity;
            var itemBTotal = itemB.UnitPrice * quantity1;

            result.Should().Be(itemATotal + itemBTotal);
        }

        [Test(Description = "Given GetTotal When value is many decimal places Then round to two correctly")]
        public void GivenGetTotalWhenTotalIsManyDecimalPlacesThenRoundToTwoCorrectly()
        {
            var itemA = new Item() {Sku = "F", UnitPrice = 13.37373m}; // 66.86865 total 
            int quantity = 5;

            var itemB = new Item() { Sku = "G", UnitPrice = 17.5959m }; // 158.3631 total
            int quantity1 = 9;

            var basket = new ShoppingBasket();

            basket.AddToBasket(itemA, quantity);
            basket.AddToBasket(itemB, quantity1);

            var result = basket.TotalPrice();

            result.Should().Be(225.23m); // 225.23175 - should be 225.23
        }

        #endregion
    }
}