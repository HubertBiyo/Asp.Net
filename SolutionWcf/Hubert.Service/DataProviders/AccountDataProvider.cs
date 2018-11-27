using Hubert.Service.IDataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hubert.Service.DataProviders
{
    public class AccountDataProvider : IAccountDataProvider
    {
        public string GetStr()
        {
            return "Good Luck";
        }

        /// <summary>
        /// 单例
        /// </summary>
        public AccountDataProvider() { }
        public static AccountDataProvider Instance { get; } = new AccountDataProvider();
    }
}
