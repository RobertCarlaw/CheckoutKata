namespace Kata.Checkout.Models
{
    public class CartItem 
    {
        public Item Item { get; }
        public int Quantity { get; private set; }
        public CartItem(Item item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
