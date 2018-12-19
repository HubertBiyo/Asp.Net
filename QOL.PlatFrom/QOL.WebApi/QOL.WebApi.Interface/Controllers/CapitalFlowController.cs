using QOL.Entities.Account;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace QOL.WebApi.Interface.Controllers
{
    public class CapitalFlowController : BaseController
    {
        [HttpGet]
        public HttpResponseMessage SearchList(int PageIndex, int PageSize, string Search)
        {
            SearchCapitalFlow condition = (new SearchCapitalFlow
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Search = Search,
            });
            var list = QOL.Service.Account.CapitalFlowService.Instance.SearchList(condition);
            return toJson(list);
        }
        public HttpResponseMessage Add(CapitalFlow model)
        {
            if (model == null)
            {
                return toJson(new { Code = -1, Message = "参数错误" });
            }
            var result = QOL.Service.Account.CapitalFlowService.Instance.Add(model);
            return toJson(result);
        }
        public HttpResponseMessage Edit(CapitalFlow model)
        {
            if (model == null)
            {
                return toJson(new { Code = -1, Message = "参数错误" });
            }
            var result = QOL.Service.Account.CapitalFlowService.Instance.Edit(model);
            return toJson(result);
        }
        public HttpResponseMessage Delete(Guid Id)
        {
            if (Id == Guid.NewGuid())
            {
                return toJson(new { Code = -1, Message = "参数错误" });
            }
            var result = QOL.Service.Account.CapitalFlowService.Instance.Delete(Id);
            return toJson(result);
        }
        
    }
}
