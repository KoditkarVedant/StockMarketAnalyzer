using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockMarketAnalyzer.Controllers
{
    public class UsersController : Controller
    {
        private readonly IServices _services;
        
        public UsersController(IServices services)
        {
            _services = services;
        }

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Profile()
        {
            var profileVM = _services.GetUserProfile(1);
            return View(profileVM);
        }

        [HttpPost]
        public ActionResult Profile(UserProfile profile)
        {
            if (!ModelState.IsValid)
            {
                var isUpdated = _services.UpdateUserProfile(profile);

                if (isUpdated)
                {
                    ViewBag.ErrorMessage = "";
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                }
            }
            
            return View(profile);
        }
    }
}