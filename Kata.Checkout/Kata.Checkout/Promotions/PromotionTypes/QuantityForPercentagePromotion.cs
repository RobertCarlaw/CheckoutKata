using System;
using Kata.Checkout.Models;

namespace Kata.Checkout.Promotions.PromotionTypes
{
    public class QuantityForPercentagePromotion : IPromotion
    {
        private readonly int _quantityDiscountApplied;
        private readonly int _percentageDiscount;
        public QuantityForPercentagePromotion(int quantityDiscountApplied, int percentageDiscount )
        {
            _quantityDiscountApplied = quantityDiscountApplied;
            _percentageDiscount = percentageDiscount;
            Name = $"{percentageDiscount} off for every {quantityDiscountApplied} purchased";
        }

        public string Name { get; }
        public void Apply(CartItem cartItem, Func<CartItem, decimal> totalBaseCalculation)
        {
            if (cartItem == null)
                throw new ArgumentNullException(nameof(CartItem));

            if (totalBaseCalculation == null)
                throw new ArgumentNullException(nameof(Func<CartItem, decimal>));

            if (cartItem.Quantity < _quantityDiscountApplied)
                cartItem.SetLineTotal(totalBaseCalculation(cartItem));
            else
            {
                var totalTimes = cartItem.Quantity / _quantityDiscountApplied;
                var remainder = cartItem.Quantity % _quantityDiscountApplied;

                decimal percentage = (decimal) _percentageDiscount/100;

                var discount = (cartItem.Item.UnitPrice * _quantityDiscountApplied) * percentage;

                var subTotal = (totalTimes * (cartItem.Item.UnitPrice * _quantityDiscountApplied)) - discount * totalTimes;

                if (remainder > 0)
                {
                    subTotal = subTotal + cartItem.Item.UnitPrice * remainder;
                }

                cartItem.SetLineTotal(subTotal);
            }
        }
    }
}
