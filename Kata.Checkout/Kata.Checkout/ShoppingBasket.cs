using System;
using Kata.Checkout.Models;
using System.Collections.Generic;

namespace Kata.Checkout
{
    public class ShoppingBasket
    {
        private readonly List<CartItem> _items = new List<CartItem>();

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

            _items.Add(new CartItem(item, quantity));
        }

        public IEnumerable<CartItem> GetBasket()
        {
            return _items;
        }
    }
}