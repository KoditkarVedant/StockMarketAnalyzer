using StockMarketAnalyzer.BLL.Interfaces;
using StockMarketAnalyzer.BO;
using StockMarketAnalyzer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockMarketAnalyzer.Controllers
{
    public class FacebookController : Controller
    {
        private readonly IFacebook _Facebook;
        private readonly IServices _services;

        //Injecting Dependencies
        public FacebookController(IFacebook facebook, IServices services)
        {
            _Facebook = facebook;
            _services = services;
        }

        //Update the data in database and retuns the partial view containing new facebook handle
        public ActionResult UpdateHandle(string ticker, string facebookUrl, string name)
        {

            var FBModel = new FacebookWidgetViewModel
            {
                IsAdminUser = true,
                Name = name,
                FacebookURL = facebookUrl
            };
            return PartialView("~/Views/Shared/_FacebookWidget.cshtml", FBModel);
        }

        [HttpPost]
        public JsonResult UpdateFBHandleInDatabase(FacebookWidgetViewModel model)
        {
            _services.UpdateFBHandle(model.Ticker, model.FacebookURL);
            return Json("");
        }

        /// <summary>
        /// Get the handles for the name
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public JsonResult GetHandles(string query)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(query))
            {
                /*Get Handles*/
                data = _Facebook.GetFacebookHandle(query);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Posts related to the facebook Page.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public JsonResult GetPosts(string handle)
        {
            object data = null;
            string AppAccessToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                /*Seperating the handle from facebook url*/
                char[] parameter = { '/' };

                //Check if handle is like http://www.facebook.com/handle/
                //if yes then remove last '/'
                if (handle.Length == handle.LastIndexOf('/') + 1)
                {
                    handle = handle.Remove(handle.LastIndexOf('/'));
                }

                handle = handle.Split(parameter).LastOrDefault().ToString();

                /*Get Posts*/
                data = _Facebook.GetFacebookPosts(handle);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Posts tagged to the facebook Page.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public JsonResult GetTaggedPosts(string handle)
        {
            object data = null;
            string AppAccessToken = string.Empty;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                /*Seperating the handle from facebook url*/
                char[] parameter = { '/' };

                //Check if handle is like http://www.facebook.com/handle/
                //if yes then remove last '/'
                if (handle.Length == handle.LastIndexOf('/') + 1)
                {
                    handle = handle.Remove(handle.LastIndexOf('/'));
                }

                handle = handle.Split(parameter).LastOrDefault().ToString();

                /*Get Posts*/
                data = _Facebook.GetFacebookPostsTagged(handle);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMoreHandles(string query)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(query))
            {
                /*Get Handles*/
                data = _Facebook.GetMoreFacebookHandles(query);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMorePosts(string query)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(query))
            {
                /*Get Handles*/
                data = _Facebook.GetMoreFacebookPosts(query);
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSpecificHandle(string handle)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(handle))
            {
                data = _Facebook.GetSpecificFacebookHandle(handle);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}