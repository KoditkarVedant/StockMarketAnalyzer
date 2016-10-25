using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.Filters;

namespace StockMarketAnalyzer.Areas.Security.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IServices _services;

        public AuthController(IServices services)
        {
            _services = services;
        }

        // GET: Auth
        public ActionResult Index()
        {
            return Login();
        }

        [AutherizationFilter]
        public ActionResult Login()
        {
            return View();
        }

        [AutherizationFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(User user, string returnUrl)
        {
            if (!ModelState.IsValid) return View();

            //Update this Login code with Entity
            if (_services.Authenticate(user))
            {
                var identity = new ClaimsIdentity(
                    new[]{
                        new Claim(ClaimTypes.Email,user.EmailAddress),
                        new Claim(ClaimTypes.Name,_services.GetUserId(user.EmailAddress).ToString())
                }, @"ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                identity.AddClaim(new Claim(ClaimTypes.Role, _services.GetUserRole(user.EmailAddress)));

                authManager.SignIn(identity);

                return Redirect(GetRedirectUrl(returnUrl));
            }

            ModelState.AddModelError("", "Invalid email or password");

            return View();
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home", new { area = "" });
            }

            return returnUrl;
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            return RedirectToAction("index", "home", new { area = "" });
        }

        [AutherizationFilter]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AutherizationFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(Register user)
        {
            if (!ModelState.IsValid) return View(user);

            user.UserType = UserType.User;
            user.ProfilePic = "~/Uploads/ProfilePic/Default/default_profile.png";

            if (_services.RegisterNewUser(user))
            {
                ViewBag.Success = "Registered Successfully ! you may login now.";
                return View();
            }

            ViewBag.Error= "Something went wrong please try again!";
            return View(user);
        }
    }
}