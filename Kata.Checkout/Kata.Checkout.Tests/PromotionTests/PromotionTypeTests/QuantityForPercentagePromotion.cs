using System;
using FluentAssertions;
using Kata.Checkout.Models;
using Kata.Checkout.Promotions.PromotionTypes;
using NUnit.Framework;

namespace Kata.Checkout.Tests.PromotionTests.PromotionTypeTests
{
    public class QuantityForPercentagePromotionsTests
    {
        private Func<CartItem, decimal> _calculate;

        [SetUp]
        public void SetUp()
        {
            decimal BaseCalculation(CartItem c) => c.Item.UnitPrice * c.Quantity;
            _calculate = BaseCalculation;
        }

        [Test (Description = "Given Apply When Item is null Then throw exception")]
        public void GivenApplyWhenItemIsNullThenReturn()
        {
            var sut = new QuantityForPercentagePromotion(1,1);
            Assert.Throws<ArgumentNullException>(() => sut.Apply(null,null));
        }

        [Test(Description = "Given Apply When CartItem Quantity threshold not met Then Return Correctly")]
        public void GiveApplyWhenCartItemQuantityThresholdNotMetThenReturnCorrectly()
        {
            var discount = 28m;
            var cartItem = new CartItem(new Item() { Sku = "1", UnitPrice = 10.00m }, 3);
           
            var sut = new QuantityForPercentagePromotion(4, 50);

            sut.Apply(cartItem, _calculate);

            cartItem.LineTotal.Should().Be(30.00m);
        }

        [Test(Description = "Given Apply When called Then Return Correctly")]
        [TestCase("10.00", 3, 50, 4, "30.00")]
        [TestCase("10.00", 2, 25, 2, "15.00")]
        [TestCase("10.00", 4, 25, 2, "30.00")]
        [TestCase("10.00", 5, 25, 2, "40.00")]
        public void GiveApplyWhenCalledThenReturnCorrectly(decimal unitPrice, int cartItemQuantity, int percentageDiscount,
            int quantityAppliedAt, decimal expectedTotal)
        {
            var cartItem = new CartItem(new Item() {Sku = "1", UnitPrice = unitPrice}, cartItemQuantity);

            var sut = new QuantityForPercentagePromotion(quantityAppliedAt, percentageDiscount);

            sut.Apply(cartItem, _calculate);

            cartItem.LineTotal.Should().Be(expectedTotal);
        }
    }
}
