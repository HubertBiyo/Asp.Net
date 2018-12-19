using QOL.Common.Tools;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QOL.UI.PMS.Controllers
{
    public class BaseController : Controller
    {
        public User User { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                var loginKey = CookieHelper.ReadCookie("LoginKey");
                if (string.IsNullOrEmpty(loginKey))
                {
                    filterContext.Result = RedirectToRoute("Default", new { Controller = "Login", Action = "Index" });
                }
                else
                {
                    var user = QOL.Service.Account.UserService.Instance.GetUser(Guid.Parse(loginKey));
                    if (user != null)
                    {
                        User = user;


                    }
                    else
                    {
                        filterContext.Result = RedirectToRoute("Default", new { Controller = "Login", Action = "Index" });
                    }

                }
            }
            catch (Exception e)
            {
                filterContext.Result = RedirectToRoute("Default", new { Controller = "Login", Action = "Index" });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}