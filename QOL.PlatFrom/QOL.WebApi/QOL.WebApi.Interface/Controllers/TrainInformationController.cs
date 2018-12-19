using QOL.Entities.Account;
using System;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace QOL.WebApi.Interface.Controllers
{
    public class TrainInformationController : BaseController
    {
        /// <summary>
        /// 查询火车订单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SearchList(int PageIndex, int PageSize, string Search)
        {
            SearchTrainInformation condition = (new SearchTrainInformation
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                Search = Search,
            });
            var list = QOL.Service.Account.TrainInformationService.Instance.SearchList(condition);
            return toJson(list);
        }
        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Add(TrainInformation model)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.Add(model);
            return toJson(result);
        }
        /// <summary>
        /// 编辑订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage Edit(TrainInformation model)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.Edit(model);
            return toJson(result);
        }
        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HttpResponseMessage Delete(Guid Id)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.Delete(Id);
            return toJson(result);
        }
    }

}
