using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Tests.EasyFactory;

namespace Tests
{
    [TestClass]
    public class BasicFxTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BasicFx_CtorSvc_ThrowsOnNullSvc()
        {
            // Act
            var fx = new BasicFx(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BasicFx_Calculate_ThrowsOnNullStockPrices()
        {
            // Arrange
            var fx = new BasicFx();

            // Act
            fx.Calculate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BasicFx_Calculate_ThrowsWhenStockPricesCountLessThanTwo()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[]
            {
                sp(1, 1)
            };
            
            // Act
            fx.Calculate(stockPrices);
        }

        [TestMethod]
        public void BasicFx_Calculate_CallsBasicServiceMax()
        {
            // Arrange
            var svc = Substitute.ForPartsOf<BasicService>();
            var sb = new StringBuilder();
            var count = 0;
            svc.When(x => x.Max(Arg.Any<IEnumerable<StockPrice>>()))
                .Do(info => {
                    sb.Append(ToString(info.ArgAt<IEnumerable<StockPrice>>(0)));
                    count++; });
            var fx = new BasicFx(svc);
            var stockPrices = new[]
            {
                sp(1,1),
                sp(2,2),
                sp(3,3)
            };

            // Act
            fx.Calculate(stockPrices);

            // Assert
            Assert.AreEqual(3, count);  // Gets called for each StockPrice.
            Assert.AreEqual(
                "(2, 2)(3, 3);(3, 3);empty;",
                sb.ToString());
        }

        private string ToString(IEnumerable<StockPrice> stockPrices)
        {
            if (stockPrices == null) return "null;";
            if (stockPrices.Count() == 0) return "empty;";

            var sb = new StringBuilder();
            foreach (var stockPrice in stockPrices)
            {
                sb.Append(ToString(stockPrice));
            }
            sb.Append(";");
            return sb.ToString();
        }

        private string ToString(StockPrice stockPrice)
        {
            return $"({stockPrice.DateTime.Day}, {stockPrice.Price})";
        }

        [TestMethod]
        public void BasicFx_Calculate_CallsChooseBestOrder()
        {
            // Arrange
            var svc = Substitute.ForPartsOf<BasicService>();
            var sb = new StringBuilder();
            var count = 0;
            svc.When(x => x.ChooseBestOrder(Arg.Any<Order>(), Arg.Any<Order>()))
                .Do(info => {
                    sb.Append(ToString(info.ArgAt<Order>(0)));
                    sb.Append(ToString(info.ArgAt<Order>(1)));
                    sb.Append(';');
                    count++; });
            var fx = new BasicFx(svc);
            var stockPrices = new[]
            {
                sp(1,1),
                sp(2,2),
                sp(3,3)
            };

            // Act
            fx.Calculate(stockPrices);

            // Assert
            Assert.AreEqual(2, count);  // Doesn't get called for the last StockPrice.
            Assert.AreEqual(
                "(null)(1,1,3,3);(1,1,3,3)(2,2,3,3);",
                sb.ToString());
        }

        private static string ToString(Order order)
        {
            if (order == null)
            {
                return "(null)";
            }
            else
            {
                return $"({order.Buy.DateTime.Day},{order.Buy.Price},{order.Sell.DateTime.Day},{order.Sell.Price})";
            }
        }

        [TestMethod]
        public void BasicFx_Calculate_Returns()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[] 
            {
                sp(1,1),
                sp(2,2),
                sp(3,3)
            };

            // Act
            var result = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(
                sp(1, 1),
                result.Buy);
            Assert.AreEqual(
                sp(3, 3),
                result.Sell);
        }
    }
}