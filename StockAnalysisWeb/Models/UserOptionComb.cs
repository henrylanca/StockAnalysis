using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using TradingWizard.StrategyAnalysis.OptionLib;

namespace TradingWizard.StrategyAnalysis.WebUI.Models
{
    public class UserOptionComb : OptionComb
    {
        public OptionCombination Options { get; set; }

        //public OptionCombination Options
        //{
        //    get
        //    {
        //        if (this._optionCombinations==null)
        //        {
        //            if (this.OptionCombinationXML.Length > 0)
        //            {
        //                var ser = new XmlSerializer(typeof(OptionCombination));

        //                this._optionCombinations = (OptionCombination)ser.Deserialize(new StringReader(this.OptionCombinationXML));
        //            }
        //        }

        //        return this._optionCombinations;
        //    }

        //    set
        //    {
        //        this._optionCombinations = value;

        //        if (this._optionCombinations != null)
        //        {
        //            var xs = new XmlSerializer(this._optionCombinations.GetType());
        //            var xml = new StringWriter();
        //            xs.Serialize(xml, this._optionCombinations);

        //            this.OptionCombinationXML = xml.ToString();
        //        }
        //    }
        //}

        public UserOptionComb(OptionComb optComb)
        {
            this.OptionCombID = optComb.OptionCombID;
            this.UserID = optComb.UserID;
            this.Name = optComb.Name;
            this.OptionCombinationXML = optComb.OptionCombinationXML;
            this.Description = optComb.Description;
            this.Options = null;

            if (!string.IsNullOrEmpty(this.OptionCombinationXML))
            {
                var ser = new XmlSerializer(typeof(OptionCombination));

                this.Options = (OptionCombination)ser.Deserialize(new StringReader(this.OptionCombinationXML));

            }
        }
    }
}