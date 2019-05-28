using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxProfit
{
    public class EfficientFx : ICalculateMaxProfit
    {
        private readonly EfficientService svc;

        public EfficientFx() : this(new EfficientService()) { }

        public EfficientFx(EfficientService svc)
        {
            this.svc = svc ?? throw new ArgumentNullException(nameof(svc));
        }

        public Order Calculate(IEnumerable<StockPrice> stockPrices)
        {
            if (stockPrices == null) throw new ArgumentNullException(nameof(stockPrices));
            if (stockPrices.Count() < 2) throw new ArgumentException("'stockPrices' must contain at least two values.");

            var priceArray = stockPrices.OrderBy(x=>x.DateTime).ToArray();
            var sellPrices = svc.GetSellPrices(priceArray);                   
            Order bestOrder = null;
            for (int buyIndex = 0; buyIndex < priceArray.Length - 1; buyIndex++)
            {
                svc.PopSellPrices(sellPrices, priceArray[buyIndex].DateTime);
                bestOrder = svc.ChooseBestOrder(
                    bestOrder, 
                    new Order(priceArray[buyIndex], sellPrices.Peek())
                    );
            }            
            return bestOrder;    
        }

        private class StockPriceComparer : IComparer<StockPrice>
        {
            public static StockPriceComparer Instance { get; } = new StockPriceComparer();

            public int Compare(StockPrice x, StockPrice y)
            {
                return DateTime.Compare(x.DateTime, y.DateTime);
            }
        }
    }
}