using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Tests.EasyFactory;

namespace Tests
{
    [TestClass]
    public class BasicServiceTests
    {
        [TestMethod]
        public void BasicService_Max_ReturnsNullWhenInputIsEmpty()
        {
            // Arrange
            var svc = new BasicService();
            var stockPrices = new StockPrice[0];

            // Act
            var result = svc.Max(stockPrices);

            // Assert
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void BasicService_Max_ReturnsGreatestOfStockPrices()
        {
            // Arrange
            var svc = new BasicService();
            var stockPrices = new[]
            {
                sp(1, 1),
                sp(2, 2),
                sp(3, 3) // This one is the greatest.
            };

            // Act
            var result = svc.Max(stockPrices);

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(sp(3, 3), result.Value);
        }

        [TestMethod]
        public void BasicService_Max_ReturnsFirstOfTwoEqualMaxValues()
        {
            // Arrange
            var svc = new BasicService();
            var stockPrices = new[]
            {
                sp(1, 1),
                sp(2, 3),   // Should return this one because it is earlier.
                sp(3, 3)    // This one is the same as the previous.
            };

            // Act
            var result = svc.Max(stockPrices);

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(sp(2, 3), result.Value);
        }
    }
}