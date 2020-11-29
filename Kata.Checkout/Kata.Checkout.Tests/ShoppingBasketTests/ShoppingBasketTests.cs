using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Kata.Checkout.Models;
using Kata.Checkout.Pricing;
using Kata.Checkout.Products;
using Moq;
using NUnit.Framework;

namespace Kata.Checkout.Tests.ShoppingBasketTests
{
    public class ShoppingBasketTests
    {
        #region Setup

        private ProductDemoService _productService;
        private Mock<IPricingEngine> _mockPricingEngine;

        [SetUp]
        public void SetUp()
        {
            _productService = new ProductDemoService();
            _mockPricingEngine = new Mock<IPricingEngine>();
        }

        #endregion

        #region Adding/Getting Basket

        [Test]
        public void GivenAddToBasketWhenCartItemsIsNullThenThrowException()
        {
            var basket = new ShoppingBasket(_mockPricingEngine.Object);

            Assert.Throws<ArgumentNullException>(() => basket.AddToBasket(null,6));
        }

        [Test]
        public void GivenAddToBasketWhenQuantityIsZeroThenThrowException()
        {
            var itemA = _productService.GetBySku("A");
            var basket = new ShoppingBasket(_mockPricingEngine.Object);

            Assert.Throws<ArgumentOutOfRangeException>(() => basket.AddToBasket(itemA, 0));
        }

        [Test (Description = "Given I have added a item to the basket When requesting basket Then item is returned correctly")]
        public void GivenIHaveAddedAItemToTheBasketThenTheItemIsInTheBasket()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 7;

            var basket = new ShoppingBasket(_mockPricingEngine.Object);
            basket.AddToBasket(itemA, quantity);

            var result = basket.GetBasket().ToList();

            result.Count().Should().Be(1);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);
        }

        [Test (Description = "Given many items are added When requesting basket Then items are returned correctly")]
        public void GivenIAddManyItemsToABasketThenItemsAreReturnedCorrectly()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 2;

            var itemB = _productService.GetBySku("B");
            int quantity1 = 3;

            var basket = new ShoppingBasket(_mockPricingEngine.Object);

            basket.AddToBasket(itemA, quantity);
            basket.AddToBasket(itemB, quantity1);

            var result = basket.GetBasket().ToList();

            result.Count().Should().Be(2);
            result[0].Item.Should().BeEquivalentTo(itemA);
            result[0].Quantity.Should().Be(quantity);
            result[1].Item.Should().BeEquivalentTo(itemB);
            result[1].Quantity.Should().Be(quantity1);
        }

        #endregion

        #region GetTotal

        [Test] 
        public void GivenMultipleItemsAreAddedToTheBasketThenTheBasketTotalShouldCalculateCorrectly()
        {
            var itemA = _productService.GetBySku("A");
            int quantity = 2;

            var itemB = _productService.GetBySku("B");
            int quantity1 = 3;

            var expectedTotal = 134.21m;

            _mockPricingEngine.Setup(a => a.Calculate(It.IsAny<List<CartItem>>())).Returns(expectedTotal);

            var basket = new ShoppingBasket(_mockPricingEngine.Object);

            basket.AddToBasket(itemA, quantity);
            basket.AddToBasket(itemB, quantity1);

            var result = basket.TotalPrice();

            var items = basket.GetBasket().ToList();

            _mockPricingEngine.Verify(a=>a.Calculate(items),Times.Once);
            result.Should().Be(expectedTotal);
        }

        #endregion
    }
}