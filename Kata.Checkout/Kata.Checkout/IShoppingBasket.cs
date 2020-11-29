using System.Collections.Generic;
using Kata.Checkout.Models;

namespace Kata.Checkout
{
    public interface IShoppingBasket
    {
        IEnumerable<CartItem> GetBasket();
        void AddToBasket(Item item, int quantity);
        decimal TotalPrice();
    }
}
