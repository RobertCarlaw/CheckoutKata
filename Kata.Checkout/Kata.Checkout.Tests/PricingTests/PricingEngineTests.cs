using System;
using System.Collections.Generic;
using FluentAssertions;
using Kata.Checkout.Models;
using Kata.Checkout.Pricing;
using Kata.Checkout.Promotions;
using Kata.Checkout.Promotions.PromotionTypes;
using Moq;
using NUnit.Framework;

namespace Kata.Checkout.Tests.PricingTests
{
    public class PricingEngineTests
    {
        private Mock<IPromotionProvider> _mockPromotionProvider;

        [SetUp]
        public void SetUp()
        {
            _mockPromotionProvider = new Mock<IPromotionProvider>();
        }

        #region  Calculate

        [Test (Description = "Given Calculate When no cart items presented Then throw exception")]
        public void GivenCalculateWhenNullIsPassedInThenThrowException()
        {
            var sut = new PricingEngine(_mockPromotionProvider.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Calculate(null) );
        }

        [Test(Description = "Given Calculate When cart item without promotion Then don't apply discount")]
        public void GivenCalculateWhenCartItemWithoutPromotionThenDontApplyDiscount()
        {
            List<CartItem> items = new List<CartItem>()
            {
                new CartItem( new Item(){Sku = "1",UnitPrice = 1.13m}, 2)
            };

            var sut = new PricingEngine(_mockPromotionProvider.Object);

            var totalPrice = sut.Calculate(items);

            totalPrice.Should().Be(2.26m);
        }

        [Test(Description = "Given Calculate When multiple cart items without promotion Then sum the line totals")]
        public void GivenCalculateWhenMultipleCartItemsWithoutPromotionThenSumTheLineTotals()
        {
            List<CartItem> items = new List<CartItem>()
            {
                new CartItem( new Item(){Sku = "1",UnitPrice = 1.13m}, 2),
                new CartItem( new Item(){Sku = "2",UnitPrice = 2.21m}, 1)
            };

            var sut = new PricingEngine(_mockPromotionProvider.Object);

            var totalPrice = sut.Calculate(items);

            totalPrice.Should().Be(4.47m);
        }

        [Test(Description = "Given Calculate When cart item with promotion Then call Apply on promotion And calculate correctly")]
        public void GivenCalculateWhenCartItemWithPromotionThenCallApply()
        {
            var cartItem = new CartItem(new Item() {Sku = "1", UnitPrice = 1.13m}, 2);
            decimal expectedTotalPrice = 77.77m;
            List<CartItem> items = new List<CartItem>()
            {
                cartItem
            };

            Mock<IPromotion> _mockPromotion = new Mock<IPromotion>();

            _mockPromotion.Setup(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>())).Callback(() => cartItem.SetLineTotal(expectedTotalPrice) );

            _mockPromotionProvider.Setup(a => a.Find(cartItem)).Returns(_mockPromotion.Object);

            var sut = new PricingEngine(_mockPromotionProvider.Object);
            var totalPrice = sut.Calculate(items);
            _mockPromotion.Verify(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>()), Times.Once);

            totalPrice.Should().Be(expectedTotalPrice);
        }

        [Test(Description = "Given Calculate When multiple CartItems some with promotions Then calculate correctly")]
        public void GivenCalculateWhenMultipleCartItemsSomeWithPromotionsThenCalculateCorrectly()
        {
            var cartItem = new CartItem(new Item() {Sku = "1", UnitPrice = 1.13m}, 1);
            var cartItem1 = new CartItem(new Item() { Sku = "2", UnitPrice = 2.63m }, 2);
            decimal promotionValueSet = 1.11m;
            decimal expectedTotalPrice = promotionValueSet + (cartItem1.Item.UnitPrice * cartItem1.Quantity);
            List<CartItem> items = new List<CartItem>()
            {
                cartItem,
                cartItem1
            };

            Mock<IPromotion> _mockPromotion = new Mock<IPromotion>();

            _mockPromotion.Setup(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>())).Callback(() => cartItem.SetLineTotal(promotionValueSet));

            _mockPromotionProvider.Setup(a => a.Find(cartItem)).Returns(_mockPromotion.Object);

            var sut = new PricingEngine(_mockPromotionProvider.Object);
            var totalPrice = sut.Calculate(items);
            _mockPromotion.Verify(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>()), Times.Once);

            totalPrice.Should().Be(expectedTotalPrice);
        }

        #endregion

        #region CalculateLineTotal

        [Test(Description = "Given CalculateLineTotal When null Then throw exception")]
        public void GivenCalculateLineTotalWhenNullThenThrowException()
        {
            var sut = new PricingEngine(_mockPromotionProvider.Object);

            Assert.Throws<ArgumentNullException>(() => sut.CalculateLineTotal(null));
        }

        [Test(Description = "Given CalculateLineTotal When no promotion Then don't apply discount")]
        public void GivenCalculateLineTotalWhenNoPromotionThenDontApplyDiscount()
        {
            var cartItem = new CartItem(new Item() {Sku = "1", UnitPrice = 1.13m}, 2);
            var sut = new PricingEngine(_mockPromotionProvider.Object);

            sut.CalculateLineTotal(cartItem);

            cartItem.LineTotal.Should().Be(2.26m);
        }

        [Test(Description = "Given CalculateLineTotal When promotion Then apply discount")]
        public void GivenCalculateLineTotalWhenPromotionThenApplyDiscount()
        {
            var cartItem = new CartItem(new Item() { Sku = "1", UnitPrice = 1.13m }, 2);
            var sut = new PricingEngine(_mockPromotionProvider.Object);
            Mock<IPromotion> _mockPromotion = new Mock<IPromotion>();
            var expectedTotalPrice = 321.21m;
            _mockPromotion.Setup(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>())).Callback(() => cartItem.SetLineTotal(expectedTotalPrice));
            _mockPromotionProvider.Setup(a => a.Find(cartItem)).Returns(_mockPromotion.Object);
            sut.CalculateLineTotal(cartItem);

            cartItem.LineTotal.Should().Be(expectedTotalPrice);
        }
        #endregion
    }
}
