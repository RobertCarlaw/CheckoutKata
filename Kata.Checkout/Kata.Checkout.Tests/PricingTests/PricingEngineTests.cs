using System;
using System.Collections.Generic;
using FluentAssertions;
using Kata.Checkout.Models;
using Kata.Checkout.Pricing;
using Kata.Checkout.Promotions;
using Moq;
using NUnit.Framework;

namespace Kata.Checkout.Tests.PricingTests
{
    public class PricingEngineTests
    {
        private Mock<ISetPricePromotion> _mockPromotion;

        [SetUp]
        public void SetUp()
        {
            _mockPromotion = new Mock<ISetPricePromotion>();
        }

        [Test (Description = "Given Calculate When no cart items presented Then throw exception")]
        public void GivenCalculateWhenNullIsPassedInThenThrowException()
        {
            var sut = new PricingEngine(_mockPromotion.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Calculate(null) );
        }

        [Test(Description = "Given Calculate When cart item without promotion Then don't apply discount")]
        public void GivenCalculateWhenCartItemWithoutPromotionThenDontApplyDiscount()
        {
            List<CartItem> items = new List<CartItem>()
            {
                new CartItem( new Item(){Sku = "1",UnitPrice = 1.13m}, 2)
            };

            var sut = new PricingEngine(_mockPromotion.Object);

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

            var sut = new PricingEngine(_mockPromotion.Object);

            var totalPrice = sut.Calculate(items);

            totalPrice.Should().Be(4.47m);
        }

        [Test(Description = "Given Calculate When cart item with promotion Then call Apply on promotion And calculate correctly")]
        public void GivenCalculateWhenCartItemWithPromotionThenCallApply()
        {
            var cartItem = new CartItem(new Item() {Sku = "B", UnitPrice = 1.13m}, 2);
            decimal expectedTotalPrice = 77.77m;
            List<CartItem> items = new List<CartItem>()
            {
                cartItem
            };

            _mockPromotion.Setup(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>())).Callback(() => cartItem.SetLineTotal(expectedTotalPrice) );

            var sut = new PricingEngine(_mockPromotion.Object);
            var totalPrice = sut.Calculate(items);
            _mockPromotion.Verify(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>()), Times.Once);

            totalPrice.Should().Be(expectedTotalPrice);
        }

        [Test(Description = "Given Calculate When multiple CartItems some with promotions Then calculate correctly")]
        public void GivemCalculateWhenMultipleCartItemsSomeWithPromotionsThenCalculateCorrectly()
        {
            var cartItem = new CartItem(new Item() {Sku = "B", UnitPrice = 1.13m}, 1);
            var cartItem1 = new CartItem(new Item() { Sku = "2", UnitPrice = 2.63m }, 2);
            decimal promotionValueSet = 1.11m;
            decimal expectedTotalPrice = promotionValueSet + (cartItem1.Item.UnitPrice * cartItem1.Quantity);
            List<CartItem> items = new List<CartItem>()
            {
                cartItem,
                cartItem1
            };

            _mockPromotion.Setup(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>())).Callback(() => cartItem.SetLineTotal(promotionValueSet));

            var sut = new PricingEngine(_mockPromotion.Object);
            var totalPrice = sut.Calculate(items);
            _mockPromotion.Verify(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>()), Times.Once);

            totalPrice.Should().Be(expectedTotalPrice);
        }


        #region CalculateLineTotal

        [Test(Description = "Given CalculateLineTotal When null Then throw exception")]
        public void GivenCalculateLineTotalWhenNullThenThrowException()
        {
            var sut = new PricingEngine(_mockPromotion.Object);

            Assert.Throws<ArgumentNullException>(() => sut.CalculateLineTotal(null));
        }

        [Test(Description = "Given CalculateLineTotal When not sku B Then don't apply discount")]
        public void GivenCalculateLineTotalWhenNotSkuBThenDontApplyDiscount()
        {
            var cartItem = new CartItem(new Item() { Sku = "1", UnitPrice = 1.13m }, 2);
            var sut = new PricingEngine(_mockPromotion.Object);

            sut.CalculateLineTotal(cartItem);

            cartItem.LineTotal.Should().Be(2.26m);
        }

        [Test(Description = "Given CalculateLineTotal When sku B Then apply discount")]
        public void GivenCalculateLineTotalWhenSkuThenApplyDiscount()
        {
            var cartItem = new CartItem(new Item() { Sku = "B", UnitPrice = 1.13m }, 2);
            var sut = new PricingEngine(_mockPromotion.Object);
            var expectedTotalPrice = 321.21m;
            _mockPromotion.Setup(a => a.Apply(cartItem, It.IsAny<Func<CartItem, decimal>>())).Callback(() => cartItem.SetLineTotal(expectedTotalPrice));
            
            sut.CalculateLineTotal(cartItem);

            cartItem.LineTotal.Should().Be(expectedTotalPrice);
        }
        #endregion
    }
}
