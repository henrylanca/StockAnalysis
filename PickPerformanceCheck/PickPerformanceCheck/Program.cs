using System;
using System.Collections.Generic;
using System.IO;
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

            ParallelOptions pOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };
            Parallel.ForEach(symbols,pOptions, symbol =>
            {
                Console.WriteLine(symbol);

                using (StockDataEntities context = new StockDataEntities())
                {
                    var picks = context.StockPicks.Where(p => p.Symbol == symbol).Select(p => p).OrderBy(p => p.PickDate).ToList();
                    var quotes = context.StockQuotes.Where(q => q.Symbol == symbol).Select(q => q).OrderBy(q => q.QuoteDate).ToList();

                    foreach (var pick in picks)
                    {
                        try
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

                            perf.PickQuote = pickQuote;
                            perf.RatioVSHigh = (double)(pickQuote/highQuote);
                            perf.RatioVSLow = (double)(pickQuote/lowQuote);

                            var m3Low = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddDays(1) && q.QuoteDate <= pick.PickDate.AddMonths(4))).OrderBy(q => q.CloseValue).Select(p => p.CloseValue).FirstOrDefault();
                            var m3High = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate >= pick.PickDate.AddDays(1) && q.QuoteDate <= pick.PickDate.AddMonths(4))).OrderByDescending(q => q.CloseValue).Select(p => p.CloseValue).FirstOrDefault();
                            var m6Low = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate > pick.PickDate.AddMonths(4) && q.QuoteDate <= pick.PickDate.AddMonths(7))).OrderBy(q => q.CloseValue).Select(p => p.CloseValue).FirstOrDefault();
                            var m6High = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate > pick.PickDate.AddMonths(4) && q.QuoteDate <= pick.PickDate.AddMonths(7))).OrderByDescending(q => q.CloseValue).Select(p => p.CloseValue).FirstOrDefault();
                            var m12Low = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate > pick.PickDate.AddMonths(6) && q.QuoteDate <= pick.PickDate.AddMonths(13))).OrderBy(q => q.CloseValue).Select(p => p.CloseValue).FirstOrDefault();
                            var m12High = quotes.Where(q => (q.TimeFrame == 1 && q.QuoteDate > pick.PickDate.AddMonths(6) && q.QuoteDate <= pick.PickDate.AddMonths(13))).OrderByDescending(q => q.CloseValue).Select(p => p.CloseValue).FirstOrDefault();

                            perf.M3Low = m3Low;
                            perf.M3High = m3High;
                            perf.M6Low = m6Low;
                            perf.M6High = m6High;
                            perf.M12Low = m12Low;
                            perf.M12High = m12High;

                            context.StockPickPerformances.Add(perf);

                        
                            context.SaveChanges();
                        }
                        catch (Exception exp)
                        {
                            //WriteLog(symbol);
                            StringBuilder errMsg = new StringBuilder();
                            while (exp != null)
                            {
                                errMsg.Append(exp.Message);
                                errMsg.Append(Environment.NewLine);

                                exp = exp.InnerException;
                            }

                            WriteLog(symbol, errMsg.ToString());
                            //Console.WriteLine("Exception for {0} - {1}", symbol, exp.Message);
                            break;
                        }
                    }

                    context.Database.Connection.Close();
                }

            });

        }

        private static void WriteLog(string symbol, string message)
        {
            string filePath = string.Format(@"c:\temp\PerformanceLog\{0}_{1:yyyyMMdd_HHmmss_ffff}.log",symbol,DateTime.Now);
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(message);
                sw.Close();
            }
        }
    }
}
