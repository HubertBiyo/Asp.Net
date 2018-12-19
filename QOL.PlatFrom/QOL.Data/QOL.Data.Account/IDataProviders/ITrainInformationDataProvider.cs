using QOL.Common.Data;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account.IDataProviders
{
    public interface ITrainInformationDataProvider : IDataProvider
    {
        bool Add(TrainInformation model);

        bool Edit(TrainInformation model);

        bool Delete(Guid Id);

        List<TrainInformation> SearchList(SearchTrainInformation condition);
        int SearchListMany(SearchTrainInformation condition);

        List<TrainInformation> SearchTrainAllList();
        bool IsHaveDataById(Guid id);
    }
}
