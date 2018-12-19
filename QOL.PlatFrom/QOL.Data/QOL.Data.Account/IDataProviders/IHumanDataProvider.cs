using QOL.Common.Data;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account.IDataProviders
{
    public interface IHumanDataProvider : IDataProvider
    {
        bool Add(Human model);

        bool Update(Human model);
        List<Human> getHumanList(SearchCondition condition);

        int getHumanMany(SearchCondition condition);

        bool Delete(Guid Id);

    }
}
