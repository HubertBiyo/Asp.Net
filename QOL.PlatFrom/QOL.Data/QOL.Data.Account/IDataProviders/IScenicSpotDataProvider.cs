using QOL.Common.Data;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account.IDataProviders
{
    public interface IScenicSpotDataProvider:IDataProvider
    {
        bool Add(ScenicSpot model);
        bool Delete(Guid Id);
        bool Edit(ScenicSpot model);

        List<ScenicSpot> SearchList(SearchScenicSpot condition);

        int SearchListMany(SearchScenicSpot condition);
    }
}
