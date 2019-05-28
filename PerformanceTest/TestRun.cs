using MaxProfit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PerformanceTest
{
    public sealed class TestRun
    {
        private const int Seed = 11;
        private const int NumSamples = 51;
        private const int SampleSize = 50;
        private const int ToMicroseconds = 1000;

        private readonly Random rand;
        private readonly List<double> results;
        private readonly ICalculateMaxProfit maxProfitFx;
        private readonly Stopwatch stopwatch;

        public string Name { get; private set; }
        
        public IEnumerable<double> Results { get => results; }

        public double Average { get => results.Average(); }

        private TestRun(string name, ICalculateMaxProfit maxProfitFx)
        {            
            this.maxProfitFx = maxProfitFx;
            rand = new Random(Seed);
            results = new List<double>();
            stopwatch = Stopwatch.StartNew();
            Name = name;
        }

        public static TestRun Execute(string name, ICalculateMaxProfit maxProfitFx)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (maxProfitFx == null) throw new ArgumentNullException(nameof(maxProfitFx));
            
            var testRun = new TestRun(name, maxProfitFx);
            for (int i = 0; i < NumSamples; i++)
            {
                testRun.Sample();
            }
            // There's something funny about the first run everytime, so throw it away.
            testRun.results.RemoveAt(0);
            return testRun;
        }

        private void Sample()
        {
            var stockPrices = NewStockPrices();
            stopwatch.Restart();
            maxProfitFx.Calculate(stockPrices);
            var elapsed = stopwatch.Elapsed.TotalMilliseconds * ToMicroseconds;
            results.Add(elapsed);    
        }

        private StockPrice[] NewStockPrices()
        {
            var startDate = new DateTime(2019, 1, 1);
            var stockPrices = new StockPrice[SampleSize];
            for (int i = 0; i < SampleSize; i++)
            {
                stockPrices[i] = new StockPrice(
                    startDate.AddDays(i),
                    (decimal)(rand.NextDouble() * 100)
                    );
            }
            return stockPrices;
        }
    }
}