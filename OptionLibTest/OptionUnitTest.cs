using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TradingWizard.StrategyAnalysis.OptionLib
{
    [TestClass]
    public class OptionUnitTest
    {
        [TestMethod]
        public void TestCalcualtePrice()
        {
            Option option = new Option();

            option.Symbol = "NFLX";
            option.Strike = 250;
            option.ExpiryDate = new DateTime(2013, 09,21);
            option.IsCall = true;

            Calculator calulator = new Calculator();
            decimal callPrice = calulator.CalcualtePrice(option, 239m, 0.003267m, new DateTime(2013, 05, 18), .5042);
            Assert.IsTrue(Math.Abs(callPrice - 23.72m) < 0.2m);

            option.IsCall = false;
            decimal putPrice = calulator.CalcualtePrice(option, 239m, 0.003267m, new DateTime(2013, 05, 18), .5042);
            Assert.IsTrue(Math.Abs(putPrice - 34.50m) < 0.2m);
        }

        [TestMethod]
        public void TestCalculatePLs()
        {
            OptionCombination optComb = new OptionCombination();

            OpenOption openOpt = new OpenOption();
            openOpt.ContractNo = 10;
            openOpt.PurchaseDate = new DateTime(2013, 05, 17);
            openOpt.PurchasePrice = 23.75m;
            openOpt.Option = new Option() { Symbol = "NFLX", IsCall = true, Strike = 250, ExpiryDate = new DateTime(2013, 09, 21) };
            optComb.Options.Add(openOpt);

            //openOpt = new OpenOption();
            //openOpt.ContractNo = -10;
            //openOpt.PurchaseDate = new DateTime(2013, 05, 17);
            //openOpt.PurchasePrice = 9.90m;
            //openOpt.Option = new Option() { Symbol = "NFLX", IsCall = true, Strike = 300, ExpiryDate = new DateTime(2013, 09, 21) };
            //optComb.Options.Add(openOpt);

            Calculator calculator = new Calculator();
            var plSerious = calculator.CalculatePLs(optComb, new DateTime(2013, 07, 01), 100m, 380m, 0.003m, 0.5);


            
        }
    }
}
