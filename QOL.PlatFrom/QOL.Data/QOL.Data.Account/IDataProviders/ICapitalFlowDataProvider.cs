using QOL.Common.Data;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account.IDataProviders
{
    public interface ICapitalFlowDataProvider : IDataProvider
    {
        bool Add(CapitalFlow model);

        bool Edit(CapitalFlow model);

        bool Delete(Guid Id);
        List<CapitalFlow> SearchList(SearchCapitalFlow condition);
        int SearchListMany(SearchCapitalFlow condition);
    }
}
