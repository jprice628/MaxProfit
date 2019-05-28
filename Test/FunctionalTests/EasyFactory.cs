using System;
using MaxProfit;

namespace Tests
{
    static class EasyFactory
    {
        public static StockPrice sp(int day, decimal price)
        {
            return new StockPrice(
                new DateTime(2019, 1, day),
                price
                );
        }

        public static Order order(int buyDay, decimal buyPrice, int sellDay, decimal sellPrice)
        {
            return new Order(
                sp(buyDay, buyPrice),
                sp(sellDay, sellPrice)
                );
        }        
    }
}