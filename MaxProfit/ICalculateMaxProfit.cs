using System.Collections.Generic;

namespace MaxProfit
{
    public interface ICalculateMaxProfit
    {
        Order Calculate(IEnumerable<StockPrice> stockPrices);
    }
}