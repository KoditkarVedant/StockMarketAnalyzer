using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockMarketAnalyzer.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IServices _services;

        public AdminController(IServices services)
        {
            _services = services;
        }

        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Notifications()
        {
            var companies = _services.GetCompaniesWithoutHandle();
            return View(companies);
        }

        public ActionResult UpdateHandle(string ticker, string name, string FBhandle, string TWhandle)
        {
            var updateHandle = new UpdateHandle()
            {
                Ticker = ticker,
                FBHandle = FBhandle,
                TwitterHandle = TWhandle,
                Name = name
            };
            return View(updateHandle);
        }
    }
}