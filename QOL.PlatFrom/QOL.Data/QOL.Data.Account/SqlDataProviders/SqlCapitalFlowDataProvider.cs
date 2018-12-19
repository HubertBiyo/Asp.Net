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
    public class SqlCapitalFlowDataProvider : ICapitalFlowDataProvider
    {
        public bool Add(CapitalFlow model)
        {
            StringBuilder sql = new StringBuilder(@"
INSERT  dbo.PB_CapitalFlow
        ( Id ,
          Code ,
          Category ,
          AccountType ,
          OutMoney ,
          OrderTime,
          Consumer ,
          Remark
        )
VALUES  ( @Id ,
          @Code ,
          @Category ,
          @AccountType ,
          @OutMoney ,
          @OrderTime,
          @Consumer ,
          @Remark  
        )
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id",model.Id);
                command.Parameters.AddWithValue("@Code", model.Code);
                command.Parameters.AddWithValue("@Category", model.Category);
                command.Parameters.AddWithValue("@AccountType", model.AccountType);
                command.Parameters.AddWithValue("@OutMoney", model.OutMoney);
                command.Parameters.AddWithValue("@OrderTime", model.OrderTime);
                command.Parameters.AddWithValue("@Consumer", model.Consumer);
                command.Parameters.AddWithNullableValue("@Remark", model.Remark);
                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public bool Delete(Guid Id)
        {
            StringBuilder sql = new StringBuilder(@"
DELETE dbo.PB_CapitalFlow WHERE Id=@Id
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", Id);
                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public bool Edit(CapitalFlow model)
        {
            StringBuilder sql = new StringBuilder(@"
UPDATE  dbo.PB_CapitalFlow
SET     Code = @Code ,
        Category = @Category ,
        AccountType = @AccountType ,
        OutMoney = @OutMoney ,
        OrderTime=@OrderTime,
        Consumer = @Consumer ,
        UpdateTime = @UpdateTime ,
        Remark = @Remark
WHERE   Id = @Id
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@Code", model.Code);
                command.Parameters.AddWithValue("@Category", model.Category);
                command.Parameters.AddWithValue("@AccountType", model.AccountType);
                command.Parameters.AddWithValue("@OutMoney", model.OutMoney);
                command.Parameters.AddWithValue("@OrderTime", model.OrderTime);
                command.Parameters.AddWithValue("@Consumer", model.Consumer);
                command.Parameters.AddWithValue("@UpdateTime", model.UpdateTime);
                command.Parameters.AddWithNullableValue("@Remark", model.Remark);

                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public List<CapitalFlow> SearchList(SearchCapitalFlow condition)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT  *
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY CreateTime DESC ) AS RowNo ,
                    Id ,
                    Code ,
                    Category ,
                    AccountType ,
                    OutMoney ,
                    OrderTime,
                    Consumer ,
                    CreateTime,
                    Remark
          FROM      dbo.PB_CapitalFlow
        ) tb
WHERE   tb.RowNo BETWEEN STR(( @PageIndex - 1 ) * @PageSize + 1)
                 AND     STR(@PageIndex * @PageSize)
");
            var list = new List<CapitalFlow>();
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@PageIndex", condition.PageIndex);
                command.Parameters.AddWithValue("@PageSize", condition.PageSize);
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        while(reader.Read())
                        {
                            list.Add(new CapitalFlow
                            {
                                Id = reader.ReadGuid("Id"),
                                Code = reader.ReadString("Code"),
                                Category = reader.ReadString("Category"),
                                AccountType = reader.ReadString("AccountType"),
                                OutMoney=reader.ReadFloat("OutMoney"),
                                OrderTime=reader.ReadDateTime("OrderTime"),
                                Consumer =reader.ReadString("Consumer"),
                                CreateTime=reader.ReadDateTime("CreateTime"),
                                Remark=reader.ReadString("Remark"),
                            });
                        }
                    }
                }
            }
            return list;
        }

        public int SearchListMany(SearchCapitalFlow condition)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT  COUNT(0) Total
FROM    dbo.PB_CapitalFlow 
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_ReadWrite))
                    {
                        if (reader.Read())
                            return reader.ReadInt("Total");
                        else
                            return 0;
                    }
                }
            }
        }
    }
}
