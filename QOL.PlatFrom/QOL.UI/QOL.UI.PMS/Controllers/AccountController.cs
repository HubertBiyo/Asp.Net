using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QOL.UI.PMS.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult AccountList(SearchHuman condition)
        {
            var list = QOL.Service.Account.HumanService.Instance.HumanList(condition);
            return Json(list);
        }
        public JsonResult Add(Human model)
        {
            var res = QOL.Service.Account.HumanService.Instance.Add(model);
            return Json(res);
        }
        public JsonResult UpdateHuman(Human model)
        {
            var result = QOL.Service.Account.HumanService.Instance.UpdateHuman(model);
            return Json(result);
        }
        public JsonResult deleteHuman(Guid Id)
        {
            var result = QOL.Service.Account.HumanService.Instance.Delete(Id);
            return Json(result);
        }

        public ActionResult TrainInfo()
        {
            return View();
        }
        #region 日常出行增删改查
        public JsonResult AddTrain(TrainInformation model)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.Add(model);
            return Json(result);
        }
        public JsonResult EditTrain(TrainInformation model)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.Edit(model);
            return Json(result);
        }
        public JsonResult DeleteTrain(Guid Id)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.Delete(Id);
            return Json(result);
        }
        public JsonResult SearchTrainList(SearchTrainInformation condition)
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.SearchList(condition);
            return Json(result);
        }
        #endregion

        #region 资金流向增删改查
        public ActionResult CapitalFlow()
        {
            return View();
        }
        public JsonResult AddCapitalFlow(CapitalFlow model)
        {
            var result = QOL.Service.Account.CapitalFlowService.Instance.Add(model);
            return Json(result);
        }
        public JsonResult DeleteCapitalFlow(Guid Id)
        {
            var result = QOL.Service.Account.CapitalFlowService.Instance.Delete(Id);
            return Json(result);
        }
        public JsonResult EditCapitalFlow(CapitalFlow model)
        {
            var result = QOL.Service.Account.CapitalFlowService.Instance.Edit(model);
            return Json(result);
        }
        public JsonResult SearchCapitalFlowList(SearchCapitalFlow condition)
        {
            var result = QOL.Service.Account.CapitalFlowService.Instance.SearchList(condition);
            return Json(result);
        }
        #endregion
    }
}