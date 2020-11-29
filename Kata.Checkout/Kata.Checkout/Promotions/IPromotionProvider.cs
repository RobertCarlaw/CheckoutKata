using Kata.Checkout.Models;
using Kata.Checkout.Promotions.PromotionTypes;

namespace Kata.Checkout.Promotions
{
    public interface IPromotionProvider
    {
        IPromotion Find(CartItem item);
    }
}
