using QOL.Common.Data;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account.IDataProviders
{
    public interface IUserDataProvider : IDataProvider
    {
        User Find(string Code);
        User Find(Guid Id);
    }
}
