using System;
using Kata.Checkout.Models;

namespace Kata.Checkout.Promotions.PromotionTypes
{
    public interface IPromotion
    {
        string Name { get; }
        void Apply(CartItem item, Func<CartItem, decimal> totalBaseCalculation);
    }
}
