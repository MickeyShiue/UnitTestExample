using NSubstitute;
using NUnit.Framework;
using System;

namespace MyService.Lib.Tests
{
    [TestFixture]
    public class OrderServiceTest
    {
        [Test]
        public void Checkout_訂單金額過低_拋出例外()
        {
            var oredeContext = new OrderEntity() { TotalCost = 99 };
            IShippingCostProvider provider = Substitute.For<IShippingCostProvider>();

            var service = new OrderService(provider);
            var ex = Assert.Throws<Exception>(() => service.Checkout(oredeContext));

            Assert.That(() => ex.Message.Contains("無法出貨"));
        }

        [Test]
        public void Checkout_訂單金額過高_拋出例外()
        {
            var oredeContext = new OrderEntity() { TotalCost = 250000 };
            IShippingCostProvider provider = Substitute.For<IShippingCostProvider>();

            var service = new OrderService(provider);
            var ex = Assert.Throws<Exception>(() => service.Checkout(oredeContext));

            Assert.That(() => ex.Message.Contains("無法線上購物"));
        }

        [Test]
        public void Checkout_郵寄正常出貨()
        {
            var oredeContext = new OrderEntity() { TotalCost = 1000, ShippingMethod = EnumShippingMethod.PostOffice };
            IShippingCostProvider provider = Substitute.For<IShippingCostProvider>();
            provider.GetShippingCost(Arg.Any<OrderEntity>()).Returns(80);

            var service = new OrderService(provider);
            service.Checkout(oredeContext);

            int actual = oredeContext.TotalWithShippingCost;
            int expected = 1080;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Checkout_黑貓正常出貨()
        {
            var oredeContext = new OrderEntity() { TotalCost = 1000, ShippingMethod = EnumShippingMethod.PostOffice };
            IShippingCostProvider provider = Substitute.For<IShippingCostProvider>();
            provider.GetShippingCost(Arg.Any<OrderEntity>()).Returns(100);

            var service = new OrderService(provider);
            service.Checkout(oredeContext);

            int actual = oredeContext.TotalWithShippingCost;
            int expected = 1100;

            Assert.AreEqual(expected, actual);
        }
    }
}
