using System;
using Kata.Checkout.Models;

namespace Kata.Checkout.Promotions
{
    public interface ISetPricePromotion
    {
        string Name { get; }
        void Apply(CartItem item, Func<CartItem, decimal> totalBaseCalculation);
    }
}
