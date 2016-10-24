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
        public static string[] Extensions = { ".png", ".jpeg", ".jpg" };

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

        [HttpGet]
        public ActionResult Profile()
        {
            var userDetails = _services.GetUserProfile(Convert.ToInt32(User.Identity.Name));


            var Genders = new SelectList(new List<Gender>(){
                new Gender(){ Name="Male"},
                new Gender(){Name="Female"}
            }, "Name", "Name", new { SelectedValue = userDetails.Gender });

            TempData["Gender"] = Genders;
            return View(userDetails);
        }

        [HttpPost]
        public ActionResult Profile(UserProfile userProfile, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid) return View(userProfile);

            var Genders = new SelectList(new List<Gender>(){
                new Gender(){ Name="Male"},
                new Gender(){Name="Female"}
            }, "Name", "Name", new { SelectedValue = userProfile.Gender });

            TempData["Gender"] = Genders;

            if (file != null)
            {
                Random r = new Random();
                long Number = r.Next(int.MaxValue);

                UploadFileEnum result = UploadFile(file, Number);

                if (result == UploadFileEnum.OK)
                {
                    string oldFileName = userProfile.ProfileUrl;

                    userProfile.ProfileUrl = "~/Uploads/ProfilePic/" + Number.ToString() + file.FileName;
                    if (_services.UpdateUserProfile(userProfile))
                    {
                        if (System.IO.File.Exists(Server.MapPath(oldFileName)))
                        {
                            System.IO.File.Delete(Server.MapPath(oldFileName));
                        }
                        ViewBag.Success = "Profile is updated successfully !";
                        return View(userProfile);
                    }
                    else
                    {
                        System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(userProfile.ProfileUrl));
                        ViewBag.Error = "Profile is not updated successfully !";
                        userProfile.ProfileUrl = oldFileName;
                        return View(userProfile);
                    }
                }
                else if (result == UploadFileEnum.FileExtensionNotSupported)
                {
                    ViewBag.Error = "Only following extensions are supported [" + string.Join(", ", Extensions) + " ]";
                }
                else if (result == UploadFileEnum.FileNotSelected)
                {
                    ViewBag.Error = "Please select the file";
                }
                else if (result == UploadFileEnum.FileSizeExceeded)
                {
                    ViewBag.Error = "File size should be less than 1 MB";
                }
                else
                {
                    ViewBag.Error = "Unexpected error occured please try again after sometime";
                }
            }
            else
            {
                if (_services.UpdateUserProfile(userProfile))
                {
                    ViewBag.Success = "Profile is updated successfully !";
                    return View(userProfile);
                }
                else
                {
                    ViewBag.Error = "Profile is not updated successfully !";
                    return View(userProfile);
                }
            }
            return View(userProfile);
        }

        public ActionResult Portfolio()
        {
            var list = _services.getPortfolio(Convert.ToInt32(User.Identity.Name));
            return View(list);
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

        public void _AddToPortfolio(UserPortfolio userPortfolio)
        {
            if (ModelState.IsValid)
            {
                userPortfolio.UserId = Convert.ToInt32(Convert.ToInt32(User.Identity.Name));
                _services.AddToProfile(userPortfolio);
            }
        }

        public void RemoveFromPortfolio(int id)
        {
            _services.RemoveFromPortfolio(id);
        }




        private static UploadFileEnum UploadFile(HttpPostedFileBase file, long Number)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (CheckExtensions(file))
                {
                    if (CheckSize(file))
                    {

                        string path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/ProfilePic/") + Number.ToString() + file.FileName;

                        file.SaveAs(path);

                        return UploadFileEnum.OK;
                    }
                    else
                    {
                        return UploadFileEnum.FileSizeExceeded;
                    }
                }
                else
                {
                    return UploadFileEnum.FileExtensionNotSupported;
                }
            }
            else
            {
                return UploadFileEnum.FileNotSelected;
            }
        }

        private static bool CheckExtensions(HttpPostedFileBase file)
        {
            string extension = Path.GetExtension(file.FileName);

            return Extensions.Contains(extension) ? true : false;
        }

        private static bool CheckSize(HttpPostedFileBase file)
        {
            double fileSize = file.ContentLength / (1024.0 * 1024.0);

            return fileSize <= 1 ? true : false;
        }
    }
}