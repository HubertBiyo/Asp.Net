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
    public class SqlHumanDataProvider : IHumanDataProvider
    {
        public bool Add(Human model)
        {
            string sql = @"INSERT  INTO PB_Human
        ( [Id] ,
          [Code] ,
          [Name] ,
          [Sex] ,
          [Birthday] ,
          [Phone] ,
          [LocalAddress]
        )
VALUES  ( @Id ,
          @Code ,
          @Name ,
          @Sex ,
          @Birthday ,
          @Phone ,
          @LocalAddress
        )";
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@Code", model.Code);
                command.Parameters.AddWithValue("@Name", model.Name);
                command.Parameters.AddWithValue("@Sex", model.Sex);
                command.Parameters.AddWithValue("@Birthday", model.Birthday);
                command.Parameters.AddWithValue("@Phone", model.Phone);
                command.Parameters.AddWithValue("@LocalAddress", model.LocalAddress);
                using (var excutor = new SqlExecutor())
                {
                    return excutor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public bool Update(Human model)
        {
            string sql = @"UPDATE  dbo.PB_Human
SET     [Name] = @Name ,
        [Sex] = @Sex ,
        [Birthday] = @Birthday ,
        [Phone] = @Phone ,
        [LocalAddress] = @LocalAddress
WHERE   Id = @Id";
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@Name", model.Name);
                command.Parameters.AddWithValue("@Sex", model.Sex);
                command.Parameters.AddWithValue("@Birthday", model.Birthday);
                command.Parameters.AddWithValue("@LocalAddress", model.LocalAddress);
                command.Parameters.AddWithValue("@Phone", model.Phone);
                using (var excutor = new SqlExecutor())
                {
                    return excutor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }
        public List<Human> getHumanList(SearchCondition condition)
        {
            string sql = @"SELECT  *
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY [Id] ) AS RowNo ,
                    [Id] ,
                    [Code] ,
                    [Name] ,
                    [Sex] ,
                    [Birthday] ,
                    [Phone] ,
                    [LocalAddress] ,
                    [OpenId] ,
                    [RoleType] ,
                    [CreateTime] ,
                    [UpdateTime] ,
                    [Status]
          FROM      [QOLPMDB].[dbo].[PB_Human]
        ) tb
WHERE   tb.RowNo BETWEEN STR(( @PageIndex - 1 ) * @PageSize + 1)
                 AND     STR(@PageIndex * @PageSize)
ORDER BY CreateTime DESC";
            var list = new List<Human>();
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@PageIndex", condition.PageIndex);
                command.Parameters.AddWithValue("@PageSize", condition.PageSize);
                using (var excutor = new SqlExecutor())
                {
                    using (var reader = excutor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        while (reader.Read())
                        {
                            list.Add(new Human
                            {
                                Id = reader.ReadGuid("Id"),
                                Code = reader.ReadString("Code"),
                                Name = reader.ReadString("Name"),
                                Sex = reader.ReadByte("Sex"),
                                Birthday = reader.ReadDateTime("Birthday"),
                                Phone = reader.ReadString("Phone"),
                                LocalAddress = reader.ReadString("LocalAddress"),
                                OpenId = reader.ReadString("OpenId"),
                                RoleType = reader.ReadInt("RoleType"),
                                CreateTime = reader.ReadDateTime("CreateTime"),
                                UpdateTime = reader.ReadDateTime("UpdateTime"),
                                Status = reader.ReadInt("Status"),
                            });
                        }

                    }
                }
            }
            return list;
        }

        public int getHumanMany(SearchCondition condition)
        {
            string sql = @"
SELECT COUNT(0) Total FROM dbo.PB_Human";
            using (var command = new SqlCommand(sql))
            {
                using (var excutor = new SqlExecutor())
                {
                    using (var reader = excutor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        if (reader.Read())
                            return reader.ReadInt("Total");
                    }
                }
            }
            return 0;
        }

        public bool Delete(Guid Id)
        {
            string sql = @"DELETE dbo.PB_Human WHERE Id=@Id ";
            using (var command = new SqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Id", Id);
                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }
    }
}
