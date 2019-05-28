using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MaxProfit;
using System.IO;

using static System.Console;

namespace PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var basic = TestRun.Execute("Basic", new BasicFx());
            PrintResults(basic);

            var efficient = TestRun.Execute("Efficient", new EfficientFx());            
            PrintResults(efficient);

            PrintAverage(basic, efficient);
        }

        private static void PrintResults(TestRun testRun)
        {
            WriteLine(testRun.Name);
            ForegroundColor = ConsoleColor.Gray;
            foreach(var result in testRun.Results)
            {
                WriteLine("- " + result);
            }
            ResetColor();
            WriteLine();
        }

        private static void PrintAverage(params TestRun[] testRuns)
        {
            ForegroundColor = ConsoleColor.Green;
            WriteLine("Averages: ");
            ResetColor();
            
            foreach(var testRun in testRuns)
            {
                WriteLine(testRun.Name + ": " + testRun.Average);
            }            
        }
    }
}
