//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TradingWizard.StrategyAnalysis.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OptionComb
    {
        public int OptionCombID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Description { get; set; }
        public string OptionCombinationXML { get; set; }
    }
}
