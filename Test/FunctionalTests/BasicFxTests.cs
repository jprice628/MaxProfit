using System;
using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Gives us aliases for creating StockPrice values and Order objects.
using static Tests.EasyFactory;

namespace Tests
{
    [TestClass]
    public class BasicFxTests
    {
        [TestMethod]
        public void BasicFx_Calculate_Typical()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[]
            {
                sp( 1, 93.50m),
                sp( 2, 94.06m),
                sp( 3, 93.23m),
                sp( 4, 91.99m), // Buy
                sp( 5, 93.06m),
                sp( 6, 92.85m),
                sp( 7, 92.31m),
                sp( 8, 93.51m),
                sp( 9, 94.35m), // Sell
                sp(10, 93.50m)
            };

            // Act
            var order = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(sp(4, 91.99m), order.Buy);
            Assert.AreEqual(sp(9, 94.35m), order.Sell);
        }

        [TestMethod]
        public void BasicFx_Calculate_Decending()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[]
            {
                sp( 1, 94.35m),
                sp( 2, 94.06m),
                sp( 3, 93.51m),
                sp( 4, 93.50m), // Buy
                sp( 5, 93.50m), // Sell -- The maximum possible profit is zero.
                sp( 6, 93.23m),
                sp( 7, 93.06m),
                sp( 8, 92.85m),
                sp( 9, 92.31m), 
                sp(10, 91.99m)
            };

            // Act
            var order = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(sp(4, 93.50m), order.Buy);
            Assert.AreEqual(sp(5, 93.50m), order.Sell);
        }
        
        [TestMethod]
        public void BasicFx_Calculate_TwoPotentialSellDates()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[]
            {
                sp(1, 91.00m),
                sp(2, 90.00m), // Buy
                sp(3, 95.00m), // Sell
                sp(4, 92.00m),
                sp(5, 95.00m)  // Don't sell here. Too late!
            };

            // Act
            var order = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(sp(2, 90.00m), order.Buy);
            Assert.AreEqual(sp(3, 95.00m), order.Sell);
        }        

        [TestMethod]
        public void BasicFx_Calculate_TwoPotentialBuyDates()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[]
            {
                sp(1, 91.00m),
                sp(2, 90.00m), // Don't buy. Too early.
                sp(3, 90.00m), // Buy
                sp(4, 92.00m),
                sp(5, 95.00m)  // Sell
            };

            // Act
            var order = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(sp(3, 90.00m), order.Buy);
            Assert.AreEqual(sp(5, 95.00m), order.Sell);
        }

        [TestMethod]
        public void BasicFx_Calculate_TwoPotentialOrders()
        {
            // Arrange
            var fx = new BasicFx();
            var stockPrices = new[]
            {
                sp(1, 91.00m),
                sp(2, 90.00m), // Buy
                sp(3, 95.00m), // Sell
                sp(4, 90.00m), // Don't buy here. Too late!
                sp(5, 95.00m)  // Don't sell here. Too late!
            };

            // Act
            var order = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(sp(2, 90.00m), order.Buy);
            Assert.AreEqual(sp(3, 95.00m), order.Sell);
        }        
    }
}