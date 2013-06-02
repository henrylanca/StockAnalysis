using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradingWizard.StrategyAnalysis.WebUI.Controllers
{
    public class OptionChartController : Controller
    {
        //
        // GET: /OptionChart/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /OptionChart/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /OptionChart/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OptionChart/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /OptionChart/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /OptionChart/Edit/5

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
        // GET: /OptionChart/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OptionChart/Delete/5

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
