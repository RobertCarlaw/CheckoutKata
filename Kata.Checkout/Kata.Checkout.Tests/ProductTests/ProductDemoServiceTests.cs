using FluentAssertions;
using Kata.Checkout.Products;
using NUnit.Framework;

namespace Kata.Checkout.Tests.ProductTests
{
    public class ProductDemoServiceTests
    {
        #region GetBySku

        [Test(Description = "Given GetBySku is called When a valid Sku is provided Then return item")]
        public void GivenGetBySkuIsCalledWhenAValidSkuIsProvidedThenReturnItem()
        {
            var expectedSku = "A";
            var sut = new ProductDemoService();

            var item = sut.GetBySku(expectedSku);

            item.Should().NotBeNull();
            item.Sku.Should().Be(expectedSku);
        }

        [Test(Description = "Given GetBySku is called When a invalid Sku is provided Then return null")]
        public void GivenGetBySkuIsCalledWhenAInvalidSkuIsProvidedThenReturnNull()
        {
            var expectedSku = "gorka";
            var sut = new ProductDemoService();

            var item = sut.GetBySku(expectedSku);

            item.Should().BeNull();
        }

        #endregion
    }
}
