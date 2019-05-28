using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

using static Tests.EasyFactory;

namespace Tests
{
    [TestClass]
    public class EfficientFxTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EfficientFx_CtorSvc_ThrowsOnNullSvc()
        {
            // Act
            var fx = new EfficientFx(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EfficientFx_Calculate_ThrowsOnNullStockPrices()
        {
            // Arrange
            var fx = new EfficientFx();

            // Act
            fx.Calculate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EfficientFx_Calculate_ThrowsWhenStockPricesCountLessThanTwo()
        {
            // Arrange
            var fx = new EfficientFx();
            var stockPrices = new[]
            {
                sp(1, 1)
            };
            
            // Act
            fx.Calculate(stockPrices);
        }

        [TestMethod]
        public void EfficientFx_Calculate_CallsGetSellPrices()
        {
            // Arrange
            var svc = Substitute.ForPartsOf<EfficientService>();
            var getSellPricesInput = string.Empty;
            svc.When(x=>x.GetSellPrices(Arg.Any<StockPrice[]>()))
                .Do(info => getSellPricesInput = ToString(info.ArgAt<StockPrice[]>(0)));
            var fx = new EfficientFx(svc);            
            var stockPrices = new[]
            {
                sp(1, 1),
                sp(2, 5),
                sp(3, 3),
                sp(4, 4),
                sp(5, 7)
            };

            // Act
            fx.Calculate(stockPrices);

            // Assert
            svc.Received().GetSellPrices(Arg.Any<StockPrice[]>());
            Assert.AreEqual(
                "(1, 1)(2, 5)(3, 3)(4, 4)(5, 7)",
                getSellPricesInput);
        }

        [TestMethod]
        public void EfficientFx_Calculate_SortsStockPrices()
        {
            // We can tell whether or not the StockPrices have been sorted by
            // observing what has been passed to the GetSellPrices method.

            // Arrange
            var svc = Substitute.ForPartsOf<EfficientService>();
            var getSellPricesInput = string.Empty;
            svc.When(x=>x.GetSellPrices(Arg.Any<StockPrice[]>()))
                .Do(info => getSellPricesInput = ToString(info.ArgAt<StockPrice[]>(0)));
            var fx = new EfficientFx(svc);            
            var stockPrices = new[]
            {
                sp(4, 1),   // Prices don't matter here.
                sp(1, 1),
                sp(3, 1),
                sp(2, 1),
                sp(5, 1)
            };

            // Act
            fx.Calculate(stockPrices);

            // Assert
            Assert.AreEqual(
                "(1, 1)(2, 1)(3, 1)(4, 1)(5, 1)",
                getSellPricesInput);
        }

        private static string ToString(StockPrice[] stockPrices)
        {
            return string.Concat(stockPrices.Select(s => ToString(s)));
        }

        private static string ToString(StockPrice s)
        {
            return $"({s.DateTime.Day}, {s.Price})";
        }

        [TestMethod]
        public void EfficientFx_Calculate_CallsPopSellPrices()
        {
            // Arrange
            var svc = Substitute.ForPartsOf<EfficientService>();
            var sb = new StringBuilder();
            svc.When(x => x.PopSellPrices(Arg.Any<Stack<StockPrice>>(), Arg.Any<DateTime>()))
                .Do(info => 
                    sb.Append(info.ArgAt<DateTime>(1).ToString("yyyy-MM-dd"))
                        .Append(";")
                    );
            var fx = new EfficientFx(svc);            
            var stockPrices = new[]
            {
                sp(1, 1),
                sp(2, 5),
                sp(3, 3),
                sp(4, 4),
                // An order can't be created from buying on the last date in 
                // the list, so this one shouldn't be present.
                sp(5, 7) 
            };

            // Act
            fx.Calculate(stockPrices);

            // Assert
            Assert.AreEqual(
                "2019-01-01;2019-01-02;2019-01-03;2019-01-04;", 
                sb.ToString()
                );
        }        

        [TestMethod]
        public void EfficientFx_Calculate_CallsChooseBestOrder()
        {
            // Arrange
            var svc = Substitute.ForPartsOf<EfficientService>();
            var sb = new StringBuilder();
            svc.When(x => x.ChooseBestOrder(Arg.Any<Order>(), Arg.Any<Order>()))
                .Do(info => {
                    sb.Append(ToShortString(info.ArgAt<Order>(0)))
                        .Append(ToShortString(info.ArgAt<Order>(1)))
                        .Append(";");
                    });
            var fx = new EfficientFx(svc);            
            var stockPrices = new[]
            {
                sp(1, 1),
                sp(2, 5),
                sp(3, 3),
                sp(4, 4),
                // An order can't be created from buying on the last date in 
                // the list, so this one shouldn't be present.
                sp(5, 7) 
            };

            // Act
            fx.Calculate(stockPrices);

            // Assert
            Assert.AreEqual(
                "(null)(1,1,5,7);(1,1,5,7)(2,5,5,7);(1,1,5,7)(3,3,5,7);(1,1,5,7)(4,4,5,7);", 
                sb.ToString()
                );
        }                

        [TestMethod]
        public void EfficientFx_Calculate_Returns()
        {
            // Arrange
            var svc = Substitute.ForPartsOf<EfficientService>();
            var sb = new StringBuilder();
            svc.When(x => x.ChooseBestOrder(Arg.Any<Order>(), Arg.Any<Order>()))
                .Do(info => {
                    sb.Append(ToShortString(info.ArgAt<Order>(0)))
                        .Append(ToShortString(info.ArgAt<Order>(1)))
                        .Append(";");
                    });
            var fx = new EfficientFx(svc);            
            var stockPrices = new[]
            {
                sp(1, 1),
                sp(2, 5),
                sp(3, 3),
                sp(4, 4),
                // An order can't be created from buying on the last date in 
                // the list, so this one shouldn't be present.
                sp(5, 7) 
            };

            // Act
            var result = fx.Calculate(stockPrices);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(sp(1, 1), result.Buy);
            Assert.AreEqual(sp(5, 7), result.Sell);
        }

        private static string ToShortString(Order order)
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
    }
}