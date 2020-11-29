using System;

namespace Kata.Checkout.Helpers
{
    internal static class PricingHelper
    {
        public static decimal RoundPrice(decimal price)
        {
            return Math.Round(price, 2, MidpointRounding.AwayFromZero);
        }
    }
}
