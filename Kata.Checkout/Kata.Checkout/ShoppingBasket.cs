using System;
using Kata.Checkout.Models;
using System.Collections.Generic;
using System.Linq;
using Kata.Checkout.Helpers;
using Kata.Checkout.Pricing;

namespace Kata.Checkout
{
    public class ShoppingBasket : IShoppingBasket
    {
        private readonly IPricingEngine _pricingEngine;
        private readonly List<CartItem> _items = new List<CartItem>();

        public ShoppingBasket(IPricingEngine pricingEngine)
        {
            _pricingEngine = pricingEngine;
        }

        public void AddToBasket(Item item, int quantity)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException( nameof(quantity), "must be greater than 0");
            }

            var cartItem = new CartItem(item, quantity);
            _pricingEngine.CalculateLineTotal(cartItem);

            _items.Add(cartItem);
        }

        public IEnumerable<CartItem> GetBasket()
        {
            return _items;
        }
        public decimal TotalPrice()
        {
            return _pricingEngine.Calculate(_items);
        }
    }
}