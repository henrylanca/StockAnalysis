using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TradingWizard.StrategyAnalysis.WebUI.Models;

namespace TradingWizard.StrategyAnalysis.WebUI
{
    public class OptionCombController : Controller
    {
        //
        // GET: /OptionComb/

        public ActionResult Index()
        {
            TradingWizardRepository repository = new TradingWizardRepository();
            List<UserOptionComb> optionCombs = repository.GetOptionCombs();


            return View(optionCombs);
        }

        //
        // GET: /OptionComb/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /OptionComb/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OptionComb/Create

        [HttpPost]
        public ActionResult Create(/*FormCollection collection*/ OptionComb optComb)
        {
            try
            {
                TradingWizardRepository repository = new TradingWizardRepository();
                repository.AddOptionComb(optComb);

                return RedirectToAction("Index");
            }
            catch(Exception exp)
            {
                return View();
            }
        }

        //
        // GET: /OptionComb/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /OptionComb/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OptionComb/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OptionComb/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
