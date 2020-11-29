using System.Collections.Generic;
using Kata.Checkout.Models;
using Kata.Checkout.Promotions.PromotionTypes;

namespace Kata.Checkout.Promotions
{
    public class PromotionProvider : IPromotionProvider
    {
        private readonly Dictionary<string,IPromotion> _promotionMapper = new Dictionary<string, IPromotion>();

        public PromotionProvider()
        {
            _promotionMapper.Add("B", new QuantityForSetPricePromotion(3,40));
            _promotionMapper.Add("D", new QuantityForPercentagePromotion(2, 25));
        }

        public IPromotion Find(CartItem item)
        {
            return !_promotionMapper.ContainsKey(item.Item.Sku) 
                    ? null 
                    : _promotionMapper[item.Item.Sku];
        }
    }
}
