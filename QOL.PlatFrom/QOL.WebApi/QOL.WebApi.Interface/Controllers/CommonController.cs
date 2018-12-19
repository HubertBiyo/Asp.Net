using QOL.Common.Log;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace QOL.WebApi.Interface.Controllers
{
    public class CommonController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage Login(string account, string password)
        {
            if (string.IsNullOrEmpty(account) && string.IsNullOrEmpty(password))
            {
                return toJson(new { Code = -1, Message = "账户和密码都不能为空" });
            }
            var user = QOL.Service.Account.UserService.Instance.GetUser(account);

            if (user == null)
                return toJson(new { Code = -1, Message = "用户不存在" });
            if (user.Actived != 1)
                return toJson(new { Code = -1, Message = "用户已停用" });
            if (user.PassWord != QOL.Common.Security.DES.Encrypt3DES(password))
                return toJson(new { Code = -1, Message = "密码不正确" });
            return toJson(new { Code = 0, Message = "登录成功" });
        }
    }
}
