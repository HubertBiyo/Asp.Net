using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Service.Account
{
    public class UserService
    {
        public User GetUser(string Code)
        {
            if (string.IsNullOrEmpty(Code))
                return null;
            var model = QOL.Data.Account.DataProviders.UserDataProvider.Find(Code);
            return model;
        }
        public User GetUser(Guid Id)
        {
            if (Guid.Empty == Id)
                return null;
            var model = QOL.Data.Account.DataProviders.UserDataProvider.Find(Id);
            return model;
        }
        private UserService() { }
        public static UserService Instance { get; } = new UserService();
    }
}
