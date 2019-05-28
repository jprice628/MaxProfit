using System;

namespace MaxProfit
{
    public class Order
    {
        public StockPrice Buy { get; private set; }
        
        public StockPrice Sell { get; private set; }

        public decimal Profit 
        {
            get => Sell.Price - Buy.Price;
        }

        public TimeSpan Duration
        {
            get => Sell.DateTime - Buy.DateTime;
        }

        public Order(StockPrice buy, StockPrice sell)
        {
            if (sell.DateTime <= buy.DateTime) throw new ArgumentException("Sell date must be greater than buy date.");
            
            Buy = buy;
            Sell = sell;
        }
    }
}