using QOL.Common.Configuration;
using QOL.Common.Data;
using QOL.Data.Account.IDataProviders;
using QOL.Data.Account.SqlDataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account
{
    public class DataProviders
    {
        internal static string MainConnectionString_Readonly
        {
            get
            {
                string connectionString = WebConfig.GetSqlConnectionString("QOLPMDB");
                return connectionString;
            }
        }
        internal static string MainConnectionString_ReadWrite
        {
            get
            {
                string connectionString = WebConfig.GetSqlConnectionString("QOLPMDB");
                return connectionString;
            }
        }

        public static IHumanDataProvider HumanDataProvider
        {
            get { return Get<IHumanDataProvider, SqlHumanDataProvider>(); }
        }

        public static IUserDataProvider UserDataProvider
        {
            get { return Get<IUserDataProvider, SqlUserDataProvider>(); }
        }
        public static ITrainInformationDataProvider TrainInformationDataProvider
        {
            get { return Get<ITrainInformationDataProvider, SqlTrainInformationDataProvider>(); }
        }
        public static ICapitalFlowDataProvider CapitalFlowDataProvider
        {
            get { return Get<ICapitalFlowDataProvider, SqlCapitalFlowDataProvider>(); }
        }
        public static IScenicSpotDataProvider ScenicSpotDataProvider
        {
            get { return Get<IScenicSpotDataProvider, SqlScenicSpotDataProvider>(); }
        }
        private static U Get<T, U>()
            where T : IDataProvider
            where U : IDataProvider
        {
            Type interfaceType = typeof(T);
            Type classType = typeof(U);
            U instance;
            lock (_instanceDic)
            {
                if (_instanceDic.ContainsKey(interfaceType))
                {
                    instance = (U)_instanceDic[interfaceType];
                }
                else
                {
                    instance = (U)Activator.CreateInstance(classType);
                    _instanceDic.Add(interfaceType, instance);
                }
            }
            return instance;
        }

        static DataProviders()
        {
            _instanceDic = new Dictionary<Type, object>();
        }
        private static readonly Dictionary<Type, object> _instanceDic;
        private static readonly string connectionStringName = "QOLPMDB";
    }
}
