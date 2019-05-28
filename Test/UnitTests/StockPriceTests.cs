using System;
using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class StockPriceTests
    {
        [TestMethod]
        public void StockPrice_CtorDtDec_InitializesPropertyValues()
        {
            // Arrange
            var dateTime = new DateTime(2019, 1, 1);
            var price = 3.14m;

            // Act
            var stockPrice = new StockPrice(dateTime, price);

            // Assert
            Assert.AreEqual(dateTime, stockPrice.DateTime);
            Assert.AreEqual(price, stockPrice.Price);
        }

        [TestMethod]
        public void StockPrice_CtorStrDec_InitializesPropertyValues()
        {
            // Arrange
            var price = 3.14m;

            // Act
            var stockPrice = new StockPrice("01-01-2019", price);

            // Assert
            Assert.AreEqual(new DateTime(2019, 1, 1), stockPrice.DateTime);
            Assert.AreEqual(price, stockPrice.Price);
        }

        [TestMethod]
        public void StockPrice_EqualsMethod_ReturnsTrue()
        {
            // Arrange
            var stockPrice1 = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var stockPrice2 = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );

            // Act
            var result = stockPrice1.Equals(stockPrice2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StockPrice_EqualsMethod_ReturnsFalseWhenDateTimeNotEqual()
        {
            // Arrange
            var stockPrice1 = new StockPrice(
                new DateTime(2020, 1, 1),
                3.14m
                );
            var stockPrice2 = new StockPrice(
                new DateTime(2019, 1, 19), // Different DateTime value.
                3.14m
                );

            // Act
            var result = stockPrice1.Equals(stockPrice2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StockPrice_EqualsMethod_ReturnsFalseWhenPriceNotEqual()
        {
            // Arrange
            var stockPrice1 = new StockPrice(
                new DateTime(2020, 1, 1),
                6.28m
                );
            var stockPrice2 = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m // Different Price value.
                );

            // Act
            var result = stockPrice1.Equals(stockPrice2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StockPrice_GetHashCode_ReturnsValue()
        {
            // Arrange
            var stockPrice = new StockPrice(
                new DateTime(2020, 1, 1),
                6.28m
                );

            // Act
            var result = stockPrice.GetHashCode();

            // Assert
            Assert.AreEqual(-430143564, result);
        }

        // The equals operator uses the Equals method, so I don't feel the need 
        // to do exhaustive testing on it.
        [TestMethod]
        public void StockPrice_EqualsOperator_ReturnsTrue()
        {
            // Arrange
            var stockPrice1 = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var stockPrice2 = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );

            // Act
            var result = stockPrice1 == stockPrice2;

            // Assert
            Assert.IsTrue(result);
        }

        // The not-equals operator uses the Equals method, so I don't feel the 
        // need to do exhaustive testing on it.
        [TestMethod]
        public void StockPrice_NotEqualsOperator_ReturnsTrue()
        {
            // Arrange
            var stockPrice1 = new StockPrice(
                new DateTime(2019, 1, 1),
                3.14m
                );
            var stockPrice2 = new StockPrice(
                new DateTime(2019, 1, 1),
                6.28m
                );

            // Act
            var result = stockPrice1 != stockPrice2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void StockPrice_Parse_ReturnsStockPrice()
        {
            // Arrange            
            var str = "2019-04-16,95.430000,96.029999,94.870003,95.220001,95.220001,499100";
            //         ^DateTime                                ^Price

            // Act
            var stockPrice = StockPrice.Parse(str);

            // Assert
            Assert.AreEqual(
                new StockPrice(new DateTime(2019, 4, 16), 95.220001m),
                stockPrice
                );
        }

        [TestMethod]
        public void StockPrice_LoadFromFile_LoadsStockPrices()
        {
            // Arrange
            const string Path = @"..\..\..\StockPrice.LoadFromFile.Data.csv";

            // Act
            var stockPrices = StockPrice.LoadFromFile(Path);

            // Assert
            Assert.AreEqual(3, stockPrices.Length);
            Assert.AreEqual(
                new StockPrice(
                    new DateTime(2019, 4, 16),
                    95.220001m
                    ),
                stockPrices[0]
                );
            Assert.AreEqual(
                new StockPrice(
                    new DateTime(2019, 4, 17),
                    94.059998m
                    ),
                stockPrices[1]
                );
            Assert.AreEqual(
                new StockPrice(
                    new DateTime(2019, 4, 18),
                    93.230003m
                    ),
                stockPrices[2]
                );
        }
    }
}
