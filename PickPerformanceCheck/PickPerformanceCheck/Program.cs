using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickPerformanceCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StockDataEntities context = new StockDataEntities())
            {
                var symbols = context.StockPicks.Select(p => p.Symbol).Distinct().ToList();

                foreach (string symbol in symbols)
                {
                    var picks = context.StockPicks.Where(p => p.Symbol == symbol).Select(p => p).OrderBy(p=>p.PickDate).ToList();
                    var quotes = context.StockQuotes.Where(q=>q.Symbol==symbol && q.TimeFrame==1).Select(q => q).OrderBy(q=>q.QuoteDate).ToList();

                    foreach (var pick in picks)
                    {
                        StockPickPerformance perf = new StockPickPerformance();
                        perf.Symbol = pick.Symbol;
                        perf.PickDate = pick.PickDate;
                        perf.PickType = pick.PickType;

                        var hugeVolume = quotes.Where(q => q.QuoteDate < pick.PickDate).OrderByDescending(q => q.Volume).Select(q => q.Volume).FirstOrDefault();
                        var avgVolume = quotes.Where(q => q.QuoteDate < pick.PickDate).OrderByDescending(q => q.Volume).Select(q => q.Volume).Average();

                        //perf.PrevBigVolumeRatio =
                    }
                }
            }
        }
    }
}
