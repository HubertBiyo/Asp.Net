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
    public class TrainInformationService
    {
        public Result Add(TrainInformation model)
        {
            try
            {
                if (model == null)
                    return new Result(-1, "参数错误");
                model.Id = Guid.NewGuid();
                var isOk = Data.Account.DataProviders.TrainInformationDataProvider.Add(model);
                if (isOk)
                    return new Result(0, "创建成功");
                else
                    return new Result(-1, "创建失败");
            }
            catch (Exception ex)
            {
                Logger.Error("AddTrainInformation is error", GetType(), ex);
                return new Result(-1, "服务器异常");
            }
        }
       
        public Result Edit(TrainInformation model)
        {
            try
            {
                if (model == null)
                    return new Result(-1, "参数错误");
                var isOk = Data.Account.DataProviders.TrainInformationDataProvider.Edit(model);
                if (isOk)
                    return new Result(0, "编辑成功");
                else
                    return new Result(-1, "编辑失败");
            }
            catch (Exception ex)
            {
                Logger.Error("Edit TrainInfomation is error", GetType(), ex);
                return new Result(-1, "服务器异常");
                throw;
            }
        }
        public Result Delete(Guid Id)
        {
            try
            {
                if (Id == Guid.Empty)
                {
                    return new Result(-1, "参数错误");
                }
                var isOk = Data.Account.DataProviders.TrainInformationDataProvider.Delete(Id);
                if (isOk)
                    return new Result(0, "删除成功");
                else
                    return new Result(-1, "删除失败");
            }
            catch (Exception ex)
            {
                Logger.Error("Delete TrainInformation is error", GetType(), ex);
                return new Result(-1, "服务器异常");
            }
        }
        public PagingResult SearchList(SearchTrainInformation condition)
        {
            try
            {
                if (condition == null)
                    return new PagingResult(-1, "参数错误", null, 0);
                var list = Data.Account.DataProviders.TrainInformationDataProvider.SearchList(condition);
                var count = Data.Account.DataProviders.TrainInformationDataProvider.SearchListMany(condition);
                return new PagingResult(0, "查询成功", list, count);
            }
            catch (Exception ex)
            {
                Logger.Error("Search TrainInformation List is error", GetType(), ex);
                return new PagingResult(-1, "服务器异常", null, 0);
            }
        }
        public bool IsHaveDataById(Guid id)
        {
            try
            {
                return QOL.Data.Account.DataProviders.TrainInformationDataProvider.IsHaveDataById(id);
            }
            catch (Exception ex)
            {
                Logger.Error("IsHaveDataById is error", GetType(), ex);
                return false;
            }
        }
        public PagingResult SearchTrainAllList()
        {
            try
            {
                var list = QOL.Data.Account.DataProviders.TrainInformationDataProvider.SearchTrainAllList();
                return new PagingResult(0, "查询成功", list, list.Count);
            }
            catch (Exception ex)
            {
                Logger.Error("Search TrainInformation All List is error", GetType(), ex);
                return null;
            }
        }
        public TrainInformationService() { }

        public static TrainInformationService Instance { get; } = new TrainInformationService();
    }
}
