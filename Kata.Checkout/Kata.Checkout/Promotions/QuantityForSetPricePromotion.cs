using System;
using Kata.Checkout.Models;

namespace Kata.Checkout.Promotions
{
    // 3 for £40
    public class QuantityForSetPricePromotion : ISetPricePromotion
    {
        private int _quantityDiscountApplied;
        private decimal _discountedPrice;
        public string Name { get; }

        public QuantityForSetPricePromotion(int quantityDiscountApplied, decimal discountedPrice)
        {
            _quantityDiscountApplied = quantityDiscountApplied;
            _discountedPrice = discountedPrice;
            Name = $"{quantityDiscountApplied} for {discountedPrice}";
        }

        public void Apply(CartItem item,  Func<CartItem, decimal> totalBaseCalculation)
        {
            if (item == null )
                throw new ArgumentNullException(nameof(CartItem));

            if(totalBaseCalculation == null)
                throw new ArgumentNullException(nameof(Func<CartItem, decimal>));

            if (item.Quantity < _quantityDiscountApplied)
                item.SetLineTotal(totalBaseCalculation(item));
            else
            {
                var totalTimes = item.Quantity / _quantityDiscountApplied;
                var remainder = item.Quantity % _quantityDiscountApplied;

                var subTotal = totalTimes * _discountedPrice;

                if (remainder > 0)
                {
                    subTotal = subTotal + item.Item.UnitPrice * remainder;
                }

                item.SetLineTotal(subTotal);
            }
        }
    }
}
