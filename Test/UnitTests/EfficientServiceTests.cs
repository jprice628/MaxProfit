using System;
using System.Collections.Generic;
using System.Linq;
using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Tests.EasyFactory;

namespace Tests
{
    [TestClass]
    public class EfficientServiceTests
    {
        [TestMethod]
        public void EfficientService_GetSellPrices_LastStockPriceIsLast()
        {
            // Arrange
            var svc = new EfficientService();
            var stockPrices = new[]
            {
                sp(1, 1),
                // This is the highest price to the right of the first 
                // StockPrice value, so it should be first in the result 
                // set.
                sp(2, 5),
                // The last element is always the highest StockPrice value to 
                // the right of the second-to-last element, so it should 
                // always be included as the last element of the result set.
                sp(3, 3)
            };

            // Act
            var result = svc.GetSellPrices(stockPrices).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(sp(3, 3), result[1]);
        }

        [TestMethod]
        public void EfficientService_GetSellPrices_NeverIncludesFirstStockPrice()
        {
            // Arrange
            var svc = new EfficientService();
            var stockPrices = new[]
            {
                // Even though this is the highest price in the list, it 
                // should never be included in the result set. It can't be a 
                // sell price, because it is the first item in the set.
                sp(1, 10),
                sp(2, 5),
                sp(3, 3)
            };

            // Act
            var result = svc.GetSellPrices(stockPrices);

            // Assert
            Assert.IsFalse(result.Contains(sp(1, 10)));
        }

        [TestMethod]
        public void EfficientService_GetSellPrices_AddsStockPricesThatAreGreater()
        {
            // Arrange
            var svc = new EfficientService();
            var stockPrices = new[]
            {
                sp(1, 1),
                // This StockPrice value is greater than the one that follows 
                // it, so it should be included in the result set prior to the 
                // one that follows it.
                sp(2, 5),
                sp(3, 3)
            };

            // Act
            var result = svc.GetSellPrices(stockPrices).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(sp(2, 5), result[0]);
        }

        [TestMethod]
        public void EfficientService_GetSellPrices_AddsStockPricesThatAreEqual()
        {
            // Arrange
            var svc = new EfficientService();
            var stockPrices = new[]
            {
                sp(1, 1),
                // Even though this StockPrice value is equal to the one that
                // follows it, it should be included in the result set prior to
                // the one that follows it. This is because we would rather
                // sell earlier when possible.
                sp(2, 3),
                sp(3, 3)
            };

            // Act
            var result = svc.GetSellPrices(stockPrices).ToArray();

            // Assert
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(sp(2, 3), result[0]);
        }

        [TestMethod]
        public void EfficientService_PopSellPrices_PopsSellPricesThatAreLessThanOrEqualToTheBuyDate()
        {
            // Arrange
            var svc = new EfficientService();
            var sellPrices = new Stack<StockPrice>(new[]
            {
                sp(1, 1),
                sp(2, 1),
                sp(3, 1),
                // We'll use a buy date of 1/3/2019, so this should be the first 
                // element in the stack once we're done.
                sp(4, 1), 
                sp(5, 1),
                sp(6, 1),
            }.Reverse());   // It's a stack, so we have to reverse it.

            // Act
            svc.PopSellPrices(sellPrices, sp(3, 1).DateTime);

            // Assert
            Assert.AreEqual(3, sellPrices.Count);
            Assert.AreEqual(sp(4, 1), sellPrices.Pop());
            Assert.AreEqual(sp(5, 1), sellPrices.Pop());
            Assert.AreEqual(sp(6, 1), sellPrices.Pop());
        }
    }
}