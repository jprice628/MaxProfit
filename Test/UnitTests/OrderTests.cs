using System;
using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Order_Ctor_InitializesPropertyValues()
        {
            // Arrange 
            var buy = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var sell =  new StockPrice(
                new DateTime(2019, 2, 1),
                6.28m
                ) ;
            
            // Act
            var order = new Order(buy, sell);

            // Assert
            Assert.AreEqual(buy, order.Buy);
            Assert.AreEqual(sell, order.Sell);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Order_Ctor_ThrowsWhenStockPricesAreOutOfOrder()
        {
            // Arrange 
            var buy = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var sell =  new StockPrice(
                new DateTime(2018, 12, 1), // This sell date is prior to the buy date.
                6.28m
                ) ;
            
            // Act
            var order = new Order(buy, sell);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Order_Ctor_ThrowsWhenBuyAndSellDatesAreTheSame()
        {
            // Arrange 
            var buy = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var sell =  new StockPrice(
                new DateTime(2019, 1, 1), // This is equal to the buy date.
                6.28m
                ) ;
            
            // Act
            var order = new Order(buy, sell);
        }

        [TestMethod]
        public void Order_Profit_ReturnsProfit()
        {
            // Arrange 
            var buy = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var sell =  new StockPrice(
                new DateTime(2019, 2, 1),
                6.28m
                ) ;
            
            // Act
            var order = new Order(buy, sell);

            // Assert
            Assert.AreEqual(3.14m, order.Profit);
        }

        [TestMethod]
        public void Order_Duration_ReturnsDuration()
        {
            // Arrange 
            var buy = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var sell =  new StockPrice(
                new DateTime(2019, 1, 10),
                6.28m
                ) ;
            
            // Act
            var order = new Order(buy, sell);

            // Assert
            Assert.AreEqual(9, order.Duration.TotalDays);
        }
    }
}