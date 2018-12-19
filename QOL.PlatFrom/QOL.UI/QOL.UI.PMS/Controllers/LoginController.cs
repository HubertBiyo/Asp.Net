using QOL.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QOL.UI.PMS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index(string returnUrl = "")
        {
            ViewBag.CallBack = returnUrl;
            return View();
        }
        [HttpPost]
        public JsonResult Syslogin(string account, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(account) && string.IsNullOrEmpty(password))
                {
                    return Json(new { Code = -1, Message = "账户和密码都不能为空" });
                }

                var user = QOL.Service.Account.UserService.Instance.GetUser(account);

                if (user == null)
                    return Json(new { Code = -1, Message = "用户不存在" });
                if (user.Actived != 1)
                    return Json(new { Code = -1, Message = "用户已停用" });
                if (user.PassWord != QOL.Common.Security.DES.Encrypt3DES(password))
                    return Json(new { Code = -1, Message = "密码不正确" });

                //当前系统登录
                QOL.Common.Tools.CookieHelper.WriteCookie("LoginKey", user.Id.ToString(), DateTime.Now.AddYears(1));
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                Logger.Error("SysLogin is error.", GetType(), ex);
                return Json(new { Code = -1, msg = "服务器异常,请联系管理员" });
            }
        }
    }
}