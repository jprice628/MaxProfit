using System;
using System.Collections.Generic;

namespace MaxProfit
{
    // For internal use only. Provides functionality required by the 
    // EfficientFx class.
    public class EfficientService : ServiceBase
    {
        public EfficientService() { }
        
        public virtual Stack<StockPrice> GetSellPrices(StockPrice[] stockPrices)
        {
            // NOTE: Assumes that stockPrices is not null, that the set 
            // contains at least two values and that those values are in 
            // order by DateTime.

            var result = new Stack<StockPrice>();

            // The last stockPrice is always the greatest StockPrice to the 
            // right of its neighbor, so we can start with it.
            result.Push(stockPrices[stockPrices.Length - 1]);

            // "int i = stockPrices.Length - 2" because we already handled the last StockPrice.
            // "i > 0" because the first StockPrice is never a sell price.
            // "i--" because we're reading through the set from last to first.
            for (int i = stockPrices.Length - 2; i > 0; i--)
            {
                // ">=" because if its 'equal' then it's an earlier sell date which is better.
                if (stockPrices[i].Price >= result.Peek().Price)
                {
                    result.Push(stockPrices[i]);
                }
            }
            
            return result;
        }

        public virtual void PopSellPrices(Stack<StockPrice> sellPrices, DateTime buyDate)
        {
            // NOTE: Assumes the sellPrices is not null and that StockPrice 
            // values within the set are in the correct order.

            while(sellPrices.Peek().DateTime <= buyDate)
            {
                sellPrices.Pop();
            }
        }
    }
}