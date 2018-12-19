using QOL.Common.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QOL.UI.PMS.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main()
        {
            return View();
        }
        public ActionResult Config()
        {
            return View();
        }

        public ActionResult Logout()
        {
            CookieHelper.RemoveCookie("LoginKey");
            return RedirectToAction("index", "login");
        }

        public ActionResult NoAuthority()
        {
            return View();
        }

        public ActionResult ElementUI()
        {
            return View();
        }
    }
}