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
            List<string> symbols = new List<string>();

            using (StockDataEntities context = new StockDataEntities())
            {
                symbols = context.StockPicks.Select(p => p.Symbol).Distinct().ToList();
            }

            Parallel.ForEach(symbols, symbol =>
            {
                Console.WriteLine(symbol);

                using (StockDataEntities context = new StockDataEntities())
                {
                    var picks = context.StockPicks.Where(p => p.Symbol == symbol).Select(p => p).OrderBy(p => p.PickDate).ToList();
                    var quotes = context.StockQuotes.Where(q => q.Symbol == symbol).Select(q => q).OrderBy(q => q.QuoteDate).ToList();

                    foreach (var pick in picks)
                    {
                        StockPickPerformance perf = new StockPickPerformance();
                        perf.Symbol = pick.Symbol;
                        perf.PickDate = pick.PickDate;
                        perf.PickType = pick.PickType;

                        var hugeVolume = quotes.Where(q => (q.TimeFrame == 2 && q.QuoteDate < pick.PickDate && q.QuoteDate > pick.PickDate.AddMonths(-18))).OrderByDescending(q => q.Volume).Select(q => q.Volume).FirstOrDefault();
                        var avgVolume = quotes.Where(q => (q.TimeFrame == 2 && q.QuoteDate < pick.PickDate && q.QuoteDate > pick.PickDate.AddMonths(-18))).OrderByDescending(q => q.Volume).Select(q => q.Volume).Average();

                        perf.PrevBigVolumeRatio = hugeVolume / avgVolume;

                        var highQuote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate < pick.PickDate && q.QuoteDate > pick.PickDate.AddMonths(-18))).OrderByDescending(q => q.HighValue).Select(q => q.HighValue).FirstOrDefault();
                        var lowQuote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate < pick.PickDate && q.QuoteDate > pick.PickDate.AddMonths(-18))).OrderBy(q => q.LowValue).Select(q => q.LowValue).FirstOrDefault();
                        var pickQuote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate == pick.PickDate)).Select(p => p.CloseValue).FirstOrDefault();

                        perf.RatioVSHigh = (double)(pickQuote/highQuote);
                        perf.RatioVSLow = (double)(pickQuote/lowQuote);

                        var m1Quote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddMonths(1) && q.QuoteDate <= pick.PickDate.AddMonths(2))).OrderBy(q => q.QuoteDate).Select(p => p.CloseValue).FirstOrDefault();
                        var m3Quote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddMonths(3) && q.QuoteDate <= pick.PickDate.AddMonths(4))).OrderBy(q => q.QuoteDate).Select(p => p.CloseValue).FirstOrDefault();
                        var m6Quote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddMonths(6) && q.QuoteDate <= pick.PickDate.AddMonths(7))).OrderBy(q => q.QuoteDate).Select(p => p.CloseValue).FirstOrDefault();
                        var m12Quote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddMonths(12) && q.QuoteDate <= pick.PickDate.AddMonths(13))).OrderBy(q => q.QuoteDate).Select(p => p.CloseValue).FirstOrDefault();
                        var m18Quote = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddMonths(18) && q.QuoteDate <= pick.PickDate.AddMonths(19))).OrderBy(q => q.QuoteDate).Select(p => p.CloseValue).FirstOrDefault();

                        perf.M1Perf = (double)(m1Quote / pickQuote);
                        perf.M3Perf = (double)(m3Quote / pickQuote);
                        perf.M6Perf = (double)(m6Quote / pickQuote);
                        perf.M12Perf = (double)(m12Quote / pickQuote);
                        perf.M18Perf = (double)(m18Quote / pickQuote);

                        context.StockPickPerformances.Add(perf);

                        try
                        {                            
                            context.SaveChanges();
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine("Exception for {0} - {1}", symbol, exp.Message);
                            break;
                        }
                    }
                }

            });

        }
    }
}
