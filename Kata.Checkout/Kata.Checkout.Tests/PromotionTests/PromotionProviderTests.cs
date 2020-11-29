using FluentAssertions;
using Kata.Checkout.Models;
using Kata.Checkout.Promotions;
using Kata.Checkout.Promotions.PromotionTypes;
using NUnit.Framework;

namespace Kata.Checkout.Tests.PromotionTests
{
    public class PromotionProviderTests
    {
        #region Find

        [Test]
        public void GivenFindWhenMatchingPromotionFoundThenReturnPromotion()
        {
            var sut = new PromotionProvider();

            var promotion = sut.Find(new CartItem(new Item() {Sku = "D"}, 2));

            promotion.Should().NotBeNull();
            promotion.Should().BeOfType<QuantityForPercentagePromotion>();
        }

        [Test]
        public void GivenFindWhenNoMatchingPromotionFoundThenReturnNull()
        {
            var sut = new PromotionProvider();

            var promotion = sut.Find(new CartItem(new Item() { Sku = "Random" }, 2));

            promotion.Should().BeNull();
        }

        #endregion
    }
}
