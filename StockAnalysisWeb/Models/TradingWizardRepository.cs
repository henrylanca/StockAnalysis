using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingWizard.StrategyAnalysis.WebUI.Models
{
    public class TradingWizardRepository
    {
        public void AddOptionComb(OptionComb optComb)
        {
            using (TradingWizardEntities context = new TradingWizardEntities())
            {
                context.OptionCombs.Add(optComb);

                context.SaveChanges();
            }
        }

        public List<UserOptionComb> GetOptionCombs()
        {
            List<UserOptionComb> optCombs = new List<UserOptionComb>();

            using (TradingWizardEntities context = new TradingWizardEntities())
            {
                List<OptionComb> optionCombs = (from o in context.OptionCombs
                            select o).ToList();

                optCombs = optionCombs.Select(o => new UserOptionComb(o)).ToList();
            }

            return optCombs;
        }
    }
}