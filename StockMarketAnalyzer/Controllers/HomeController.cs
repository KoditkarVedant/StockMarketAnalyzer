using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace StockMarketAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServices _services;
        
        public HomeController(IServices services)
        {
            _services = services;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        [AllowAnonymous]
        public ActionResult LookupCompany(string query = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View();
            }
            else
            {
                var companyDetails = _services.GetCompanyDetails(query);
                return View("CompanyDetail", companyDetails);
            }
        }

        [AllowAnonymous]
        public JsonResult SearchCompany(string query = null)
        {
            return string.IsNullOrWhiteSpace(query) ? Json("", JsonRequestBehavior.AllowGet) : Json(_services.SearchCompany(query), JsonRequestBehavior.AllowGet);
        }

        




        
    }
}