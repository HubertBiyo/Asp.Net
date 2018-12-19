using QOL.Common.Data;
using QOL.Common.Extensions;
using QOL.Data.Account.IDataProviders;
using QOL.Entities.Account;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Data.Account.SqlDataProviders
{
    public class SqlUserDataProvider : IUserDataProvider
    {
        public User Find(string Code)
        {
            string sql= @"SELECT [Id]
      ,[Code]
      ,[Name]
      ,[HumanId]
      ,[HumanName]
      ,[PassWord]
      ,[ValidDate]
      ,[Actived]
      ,[ITAdminFlag]
      ,[IsWinAccount]
      ,[LastEpsProjId]
  FROM [QOLPMDB].[dbo].[PB_User] WHERE Code=@Code";
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Code", Code);
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        if (reader.Read())
                            return new User
                            {
                                Id = reader.ReadGuid("Id"),
                                Code = reader.ReadString("Code"),
                                Name = reader.ReadString("Name"),
                                HumanId = reader.ReadGuid("HumanId"),
                                HumanName = reader.ReadString("HumanName"),
                                PassWord = reader.ReadString("PassWord"),
                                ValidDate = reader.ReadDateTime("ValidDate"),
                                Actived = reader.ReadByte("Actived"),
                                ITAdminFlag = reader.ReadString("ITAdminFlag"),
                                IsWinAccount = reader.ReadString("IsWinAccount"),
                                LastEpsProjId = reader.ReadGuid("LastEpsProjId"),
                            };
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public User Find(Guid Id)
        {
            string sql = @"SELECT [Id]
      ,[Code]
      ,[Name]
      ,[HumanId]
      ,[HumanName]
      ,[PassWord]
      ,[ValidDate]
      ,[Actived]
      ,[ITAdminFlag]
      ,[IsWinAccount]
      ,[LastEpsProjId]
  FROM [QOLPMDB].[dbo].[PB_User] WHERE Id=@Id";
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Id", Id);
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        if (reader.Read())
                            return new User
                            {
                                Id = reader.ReadGuid("Id"),
                                Code = reader.ReadString("Code"),
                                Name = reader.ReadString("Name"),
                                HumanId = reader.ReadGuid("HumanId"),
                                HumanName = reader.ReadString("HumanName"),
                                PassWord = reader.ReadString("PassWord"),
                                ValidDate = reader.ReadDateTime("ValidDate"),
                                Actived = reader.ReadByte("Actived"),
                                ITAdminFlag = reader.ReadString("ITAdminFlag"),
                                IsWinAccount = reader.ReadString("IsWinAccount"),
                                LastEpsProjId = reader.ReadGuid("LastEpsProjId"),
                            };
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
    }
}
