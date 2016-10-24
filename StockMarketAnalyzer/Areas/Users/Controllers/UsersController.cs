using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;

namespace StockMarketAnalyzer.Areas.Users.Controllers
{
    public class UsersController : Controller
    {
        private readonly IServices _services;
        private static readonly string[] Extensions = new string[] {".png", ".jpeg", ".jpg"};

        public UsersController(IServices services)
        {
            _services = services;
        }

        // GET: Users/Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public new ActionResult Profile()
        {
            var userDetails = _services.GetUserProfile(Convert.ToInt32(User.Identity.Name));


            var genders = new SelectList(new List<Gender>(){
                new Gender(){ Name="Male"},
                new Gender(){Name="Female"}
            }, "Name", "Name", new { SelectedValue = userDetails.Gender });

            TempData["Gender"] = genders;
            return View(userDetails);
        }

        [HttpPost]
        public new ActionResult Profile(UserProfile userProfile, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid) return View(userProfile);

            var genders = new SelectList(new List<Gender>(){
                new Gender(){ Name="Male"},
                new Gender(){Name="Female"}
            }, "Name", "Name", new { SelectedValue = userProfile.Gender });

            TempData["Gender"] = genders;

            if (file != null)
            {
                Random r = new Random();
                long number = r.Next(int.MaxValue);

                UploadFileEnum result = UploadFile(file, number);

                if (result == UploadFileEnum.Ok)
                {
                    string oldFileName = userProfile.ProfileUrl;

                    userProfile.ProfileUrl = "~/Uploads/ProfilePic/" + number.ToString() + file.FileName;
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

        private static UploadFileEnum UploadFile(HttpPostedFileBase file, long number)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (CheckExtensions(file))
                {
                    if (CheckSize(file))
                    {

                        var path = System.Web.HttpContext.Current.Server.MapPath("~/Uploads/ProfilePic/") + number.ToString() + file.FileName;

                        file.SaveAs(path);

                        return UploadFileEnum.Ok;
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
            var extension = Path.GetExtension(file.FileName);

            return Extensions.Contains(extension) ? true : false;
        }

        private static bool CheckSize(HttpPostedFileBase file)
        {
            var fileSize = file.ContentLength / (1024.0 * 1024.0);

            return fileSize <= 1 ? true : false;
        }

        public ActionResult Portfolio()
        {
            var list = _services.GetPortfolio(Convert.ToInt32(User.Identity.Name));
            return View(list);
        }

        public void _AddToPortfolio(UserPortfolio userPortfolio)
        {
            if (!ModelState.IsValid) return;

            userPortfolio.UserId = Convert.ToInt32(Convert.ToInt32(User.Identity.Name));
            _services.AddToProfile(userPortfolio);
        }

        public void RemoveFromPortfolio(int id)
        {
            _services.RemoveFromPortfolio(id);
        }
    }
}