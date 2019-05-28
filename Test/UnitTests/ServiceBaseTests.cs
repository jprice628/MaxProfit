using System;
using MaxProfit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using static Tests.EasyFactory;

namespace Tests
{
    [TestClass]
    public class ServiceBaseTests
    {
        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder1Order2AreNull()
        {
            // Arrange
            var svc = new ConcreteServiceBase();
            
            // Act
            var bestOrder = svc.ChooseBestOrder(null, null);

            // Assert
            Assert.IsNull(bestOrder);
        }
        
        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder1IsNull()
        {
            // Arrange
            var svc = new ConcreteServiceBase();
            var anOrder = order(1, 1, 2, 2);
            
            // Act
            var besterOrder = svc.ChooseBestOrder(null, anOrder);

            // Assert
            Assert.AreSame(anOrder, besterOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder2IsNull()
        {
            // Arrange
            var svc = new ConcreteServiceBase();
            var anOrder = order(1, 1, 2, 2);
            
            // Act
            var bestOrder = svc.ChooseBestOrder(anOrder, null);

            // Assert
            Assert.AreSame(anOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder1ProfitIsGreater()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var hiProfitOrder = order( // Profit = 9.
                1, 1.00m,   
                2, 10.00m);
            var loProfitOrder = order(  // Profit = 1.
                1, 1.00m,
                2, 2.00m);

            // Act
            var bestOrder = svc.ChooseBestOrder(hiProfitOrder, loProfitOrder);

            // Assert
            Assert.AreSame(hiProfitOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder2ProfitIsGreater()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var hiProfitOrder = order( // Profit = 9.
                1, 1.00m,   
                2, 10.00m);
            var loProfitOrder = order(  // Profit = 1.
                1, 1.00m,
                2, 2.00m);

            // Act
            var bestOrder = svc.ChooseBestOrder(loProfitOrder, hiProfitOrder);

            // Assert
            Assert.AreSame(hiProfitOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder1DurationShorter()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var shortOrder = order( // Duration = 1day.
                1, 1.00m,           
                2, 2.00m);
            var longOrder = order(  // Duration = 8days.
                1, 1.00m,
                9, 2.00m);
            // Profits must be equal.
            Assert.AreEqual(shortOrder.Profit, longOrder.Profit);

            // Act
            var bestOrder = svc.ChooseBestOrder(shortOrder, longOrder);

            // Assert
            Assert.AreSame(shortOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder2DurationShorter()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var shortOrder = order( // Duration = 1day.
                1, 1.00m,           
                2, 2.00m);
            var longOrder = order(  // Duration = 8days.
                1, 1.00m,
                9, 2.00m);
            // Profits must be equal.
            Assert.AreEqual(shortOrder.Profit, longOrder.Profit);

            // Act
            var bestOrder = svc.ChooseBestOrder(longOrder, shortOrder);

            // Assert
            Assert.AreSame(shortOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder1IsEarlier()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var earlyOrder = order( 
                1, 1.00m,           
                2, 2.00m);
            var lateOrder = order(  
                3, 1.00m,
                4, 2.00m);
            // Profits and duration must be equal.
            Assert.AreEqual(earlyOrder.Profit, lateOrder.Profit);
            Assert.AreEqual(earlyOrder.Duration, lateOrder.Duration);

            // Act
            var bestOrder = svc.ChooseBestOrder(earlyOrder, lateOrder);

            // Assert
            Assert.AreSame(earlyOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenOrder2IsEarlier()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var earlyOrder = order( 
                1, 1.00m,           
                2, 2.00m);
            var lateOrder = order(  
                3, 1.00m,
                4, 2.00m);
            // Profits and duration must be equal.
            Assert.AreEqual(earlyOrder.Profit, lateOrder.Profit);
            Assert.AreEqual(earlyOrder.Duration, lateOrder.Duration);

            // Act
            var bestOrder = svc.ChooseBestOrder(lateOrder, earlyOrder);

            // Assert
            Assert.AreSame(earlyOrder, bestOrder);
        }

        [TestMethod]
        public void ServiceBase_ChooseBestOrder_ReturnsWhenAllThingsAreEqual()
        {
            // Arrange
            var svc = new ConcreteServiceBase();            
            var order1 = order( 
                1, 1.00m,           
                2, 2.00m);
            var order2 = order(  
                1, 1.00m,
                2, 2.00m);
            // Profits and duration must be equal.
            Assert.AreEqual(order1.Profit, order2.Profit);
            Assert.AreEqual(order1.Duration, order2.Duration);            
            Assert.AreEqual(order1.Buy.DateTime, order2.Buy.DateTime);

            // Act
            var bestOrder = svc.ChooseBestOrder(order1, order2);

            // Assert
            Assert.AreSame(order1, bestOrder);
        }

        private class ConcreteServiceBase : ServiceBase
        {
            public ConcreteServiceBase() { }
        }
    }
}