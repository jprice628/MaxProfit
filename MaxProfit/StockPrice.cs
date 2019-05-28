using System;
using System.Collections.Generic;

namespace MaxProfit
{
    public struct StockPrice
    {
        public static StockPrice MinValue = new StockPrice(DateTime.MinValue, decimal.MinValue);

        public DateTime DateTime { get; set; }

        public decimal Price { get; set; }

        public StockPrice(DateTime dateTime, decimal price)
        {
            DateTime = dateTime;
            Price = price;
        }

        public StockPrice(string dateTime, decimal price)
        {
            DateTime = DateTime.Parse(dateTime);
            Price = price;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;

            var s = (StockPrice)obj;
            return s.DateTime == DateTime &&
                s.Price == Price;
        }

        public override int GetHashCode()
        {
            // We don't care about overflow here.
            unchecked
            {
                // Could use larger prime numbers, but this is good enough for now.
                var hash = 17;
                hash = hash * 23 + DateTime.GetHashCode();
                hash = hash * 23 + Price.GetHashCode();
                return hash;
            }
        }

        public static bool operator==(StockPrice a, StockPrice b) => a.Equals(b);

        public static bool operator!=(StockPrice a, StockPrice b) => !a.Equals(b);

        // Example:
        // Date,      Open,     High,     Low,      Close,    Adj Close,Volume
        // 2019-04-16,95.430000,96.029999,94.870003,95.220001,95.220001,499100
        public static StockPrice Parse(string str)
        {
            // Note: This is not very robust. There's no validation, and it 
            // makes a lot of assumptions about the incoming data.
            var arr = str.Split(',');
            return new StockPrice(
                DateTime.Parse(arr[0]),
                decimal.Parse(arr[4])
                );
        }

        public static StockPrice[] LoadFromFile(string path)
        {
            // Note: this is not robust. It assumes happy path.
            using (var stream = System.IO.File.OpenRead(path))
            using (var reader = new System.IO.StreamReader(stream))
            {
                // Assume that the first line is a header.
                reader.ReadLine();

                var result = new List<StockPrice>();
                while (!reader.EndOfStream)
                {
                    result.Add(Parse(reader.ReadLine()));
                }
                return result.ToArray();
            }
        }

        public override string ToString() => $"({DateTime}, {Price})";
    }
}
