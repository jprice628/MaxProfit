using System;
using System.Collections.Generic;
using System.Linq;

namespace MaxProfit
{
    public class BasicFx : ICalculateMaxProfit
    {
        private readonly BasicService svc;

        public BasicFx() : this(new BasicService()) { }

        public BasicFx(BasicService svc)
        {
            this.svc = svc ?? throw new ArgumentNullException(nameof(svc));
        }

        public Order Calculate(IEnumerable<StockPrice> stockPrices)
        {
            if (stockPrices == null) throw new ArgumentNullException(nameof(stockPrices));
            if (stockPrices.Count() < 2) throw new ArgumentException("'stockPrices' must contain at least two values.");

            Order bestOrder = null;
            foreach(var buyPrice in stockPrices)
            {
                var sellPrice = svc.Max(stockPrices.Where(x => x.DateTime > buyPrice.DateTime));
                if (sellPrice.HasValue)
                {
                    var order = new Order(buyPrice, sellPrice.Value);
                    bestOrder = svc.ChooseBestOrder(bestOrder, order);
                }                
            }
            return bestOrder;
        }
    }
}