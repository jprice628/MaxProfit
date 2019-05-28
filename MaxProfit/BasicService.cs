using System;
using System.Collections.Generic;

namespace MaxProfit
{
    // For internal use only. Provides functionality required by the 
    // BasicFx class.
    public class BasicService : ServiceBase
    {
        public BasicService() { }

        public virtual StockPrice? Max(IEnumerable<StockPrice> stockPrices)
        {            
            // NOTE: Assumes that stockPrices is not null and that the values 
            // are ordered by DateTime.
            
            StockPrice? result = null;
            foreach(var stockPrice in stockPrices)
            {
                if (result == null)
                {
                    result = stockPrice;
                }
                else if (stockPrice.Price > result.Value.Price)
                {
                    result = stockPrice;
                }
            }
            return result;
        }      
    }
}