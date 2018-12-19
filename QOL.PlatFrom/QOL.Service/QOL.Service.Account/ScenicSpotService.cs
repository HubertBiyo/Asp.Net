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
    public class ScenicSpotService
    {
        public Result AddScenicSpot(ScenicSpot model)
        {
            try
            {
                if (model == null)
                    return new Result(-1, "参数错误");
                var isOk = QOL.Data.Account.DataProviders.ScenicSpotDataProvider.Add(model);
                if (isOk)
                {
                    return new Result(0, "创建成功");
                }
                return new Result(-1, "创建失败");
            }
            catch (Exception ex)
            {
                Logger.Error("AddScenicSpot is error",GetType(),ex);
                return new Result(-1, "服务器异常");
            }
        }
        public Result EditScenicSpot(ScenicSpot model)
        {
            try
            {
                if (model == null)
                {
                    return new Result(-1, "参数错误");
                }
                var isOk = QOL.Data.Account.DataProviders.ScenicSpotDataProvider.Edit(model);
                if (isOk)
                    return new Result(0, "创建成功");
                return new Result(-1, "创建失败");
            }
            catch (Exception ex)
            {
                Logger.Error("EditScenicSpot is error", GetType(), ex);
                return new Result(-1, "服务器异常");

            }
        }

        public  ScenicSpotService() { }

        public static ScenicSpotService Instance { get; } = new ScenicSpotService();
    }
}
