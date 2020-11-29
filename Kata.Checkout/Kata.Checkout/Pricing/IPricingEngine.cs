using System.Collections.Generic;
using Kata.Checkout.Models;

namespace Kata.Checkout.Pricing
{
    public interface IPricingEngine
    {
        void CalculateLineTotal(CartItem cartItem);
        decimal Calculate(List<CartItem> cartItems);

    }
}
