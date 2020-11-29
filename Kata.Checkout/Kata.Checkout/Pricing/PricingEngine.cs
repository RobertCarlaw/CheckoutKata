using System;
using System.Collections.Generic;
using System.Linq;
using Kata.Checkout.Helpers;
using Kata.Checkout.Models;
using Kata.Checkout.Promotions;

namespace Kata.Checkout.Pricing
{
    public class PricingEngine : IPricingEngine
    {
        private readonly IPromotionProvider _promotionProvider;
        public PricingEngine(IPromotionProvider promotionProvider)
        {
            _promotionProvider = promotionProvider;
        }

        public decimal Calculate(List<CartItem> cartItems)
        {
            if(cartItems==null)
                throw new ArgumentNullException(nameof(cartItems));

            foreach (var cartItem in cartItems)
            {
                CalculateLineTotal(cartItem);
            }

            var total = cartItems.Sum(a => a.LineTotal);
            return PricingHelper.RoundPrice(total);
        }

        public void CalculateLineTotal(CartItem cartItem)
        {
            if (cartItem == null)
                throw new ArgumentNullException(nameof(cartItem));

            decimal BaseCalculation(CartItem c) => c.Item.UnitPrice * c.Quantity;

            var promotion = _promotionProvider.Find(cartItem);

            if (promotion == null)
            {
                cartItem.SetLineTotal(BaseCalculation(cartItem));
            }
            else
            {
                promotion.Apply(cartItem, BaseCalculation);
            }
        }
    }
}
