using System;

namespace MaxProfit
{
    public abstract class ServiceBase
    {
        protected ServiceBase() { }

        public virtual Order ChooseBestOrder(Order order1, Order order2)
        {
            if (order1 == null && order2 == null) return null;
            if (order1 == null) return order2;
            if (order2 == null) return order1;

            if (order1.Profit > order2.Profit)
            {
                return order1;
            }
            else if (order2.Profit > order1.Profit)
            {
                return order2;
            }

            // Profits have to be equal to get to this point.

            if (order2.Duration < order1.Duration)
            {
                return order2;
            }
            else if(order1.Duration < order2.Duration)
            {
                return order1;
            }

            // Profits and durations have to be equal to get to this point.

            if (order2.Buy.DateTime < order1.Buy.DateTime) 
            {
                return order2;
            }
            else if (order1.Buy.DateTime < order2.Buy.DateTime)
            {
                return order1;
            }

            // Everything is equal, so...
            return order1;
        }  
    }
}