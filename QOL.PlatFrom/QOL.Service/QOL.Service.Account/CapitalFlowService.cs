using QOL.Common.Data;
using QOL.Common.Log;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Service.Account
{
    public class CapitalFlowService
    {
        public Result Add(CapitalFlow model)
        {
            try
            {
                if (model == null)
                    return new Result(-1, "参数错误");
                model.Code ="EM"+ DateTime.Now.ToString("yyyyMMddHHmmss") + Common.Security.Randoms.RandomNumber(1000, 9999);
                model.Id = Guid.NewGuid();
                var isOk = Data.Account.DataProviders.CapitalFlowDataProvider.Add(model);
                if (isOk)
                    return new Result(0, "创建成功");
                else
                    return new Result(-1, "创建失败");
            }
            catch (Exception ex)
            {
                Logger.Error("Add CapitalFlow is error", GetType(), ex);
                return new Result(-1, "服务器异常");
            }
        }
        public Result Edit(CapitalFlow model)
        {
            try
            {
                if (model == null)
                    return new Result(-1, "参数错误");
                model.UpdateTime = DateTime.Now;
                var isOk = Data.Account.DataProviders.CapitalFlowDataProvider.Edit(model);
                if (isOk)
                    return new Result(0, "编辑成功");
                else
                    return new Result(-1, "编辑失败");
            }
            catch (Exception ex)
            {

                Logger.Error("Edit CapitalFlow is error", GetType(), ex);
                return new Result(-1, "服务器异常");
            }
        }
        public Result Delete(Guid Id)
        {
            try
            {
                if (Id == Guid.Empty)
                    return new Result(-1, "参数错误");
                var isOk = Data.Account.DataProviders.CapitalFlowDataProvider.Delete(Id);
                if (isOk)
                    return new Result(0, "删除成功");
                else
                    return new Result(-1, "删除失败");
            }
            catch (Exception ex)
            {
                Logger.Error("Delete CapitalFlow is error", GetType(), ex);
                return new Result(-1, "服务器异常");
            }
        }
        public PagingResult SearchList( SearchCapitalFlow condition)
        {
            try
            {
                if (condition == null)
                    return new PagingResult(-1, "参数错误", null, 0);
                var list = Data.Account.DataProviders.CapitalFlowDataProvider.SearchList(condition);
                var count = Data.Account.DataProviders.CapitalFlowDataProvider.SearchListMany(condition);
                return new PagingResult(0, "查询成功", list, count);
            }
            catch (Exception ex)
            {
                Logger.Error("SearchList CapitalFlow is error", GetType(), ex);
                return new PagingResult(-1, "服务器异常",null,0);
            }
        }
        public List<CapitalFlow> SearchListApi(SearchCapitalFlow condition)
        {
            try
            {
                var list = Data.Account.DataProviders.CapitalFlowDataProvider.SearchList(condition);
                return list;
            }
            catch (Exception e)
            {
                Logger.Error("SearchListApi is error", GetType(), e);
                return null;
            }
        }
        public CapitalFlowService() { }
        public static CapitalFlowService Instance { get; } = new CapitalFlowService();
    }
}
