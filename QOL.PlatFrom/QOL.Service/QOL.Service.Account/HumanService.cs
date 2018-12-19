using QOL.Common.Data;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Service.Account
{
    public class HumanService
    {

        public Result Add(Human model)
        {
            if (model == null)
            {
                return new Result(-1, "参数错误");
            }
            model.Id = Guid.NewGuid();
            model.Code = DateTime.Now.ToString("yyyyMMddHHmmss") + Common.Security.Randoms.RandomNumber(100, 999);
            var isOk = QOL.Data.Account.DataProviders.HumanDataProvider.Add(model);
            return isOk ? new Result(0, "创建成功") : new Result(-1, "创建失败");
        }
        public PagingResult HumanList(SearchHuman condition)
        {
            var list = QOL.Data.Account.DataProviders.HumanDataProvider.getHumanList(condition);
            var count = QOL.Data.Account.DataProviders.HumanDataProvider.getHumanMany(condition);
            return list != null ? new PagingResult(0, "查询成功", list, count) : new PagingResult(-1, "查询失败", null, 0);
        }
        public Result Delete(Guid Id)
        {
            try
            {
                if (Guid.Empty == Id)
                    return new Result(-1, "参数错误");
                var isOk = QOL.Data.Account.DataProviders.HumanDataProvider.Delete(Id);
                return isOk ? new Result(0, "删除成功") : new Result(-1, "删除失败");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Result UpdateHuman (Human model)
        {
            try
            {
                if (model == null || Guid.Empty == model.Id)
                    return new Result(-1, "参数错误");
                var isOk = QOL.Data.Account.DataProviders.HumanDataProvider.Update(model);
                return isOk ? new Result(0, "更新成功") : new Result(-1, "更新失败");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private static readonly HumanService _instance = new HumanService();
        public static HumanService Instance
        {
            get { return _instance; }
        }

    }
}
