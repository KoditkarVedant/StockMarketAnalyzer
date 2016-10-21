using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace StockMarketAnalyzer.Controllers
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
            return View();
        }

        [AutherizationFilter]
        public ActionResult Login()
        {
            return View();
        }

        [AutherizationFilter]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(User user, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            //Update this Login code with Entity
            if (_services.Authenticate(user))
            {
                var identity = new ClaimsIdentity(
                    new[]{
                        new Claim(ClaimTypes.Email,user.EmailAddress),
                        new Claim(ClaimTypes.Name,_services.getUserID(user.EmailAddress).ToString())
                }, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                identity.AddClaim(new Claim(ClaimTypes.Role, _services.getUserRole(user.EmailAddress)));

                authManager.SignIn(identity);
                
                return Redirect(GetRedirectUrl(ReturnUrl));
            }

            ModelState.AddModelError("", "Invalid email or password");

            return View();
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");

            return RedirectToAction("index", "home");
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
            if (_services.RegisterNewUser(user))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong please try again!");
                return View(user);
            }
        }
    }
}