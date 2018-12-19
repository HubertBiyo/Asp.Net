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
    public class SqlScenicSpotDataProvider : IScenicSpotDataProvider
    {
        public bool Add(ScenicSpot model)
        {
            StringBuilder sql = new StringBuilder(@"
INSERT  INTO dbo.PB_ScenicSpot
        ( Id ,
          ScenicSpotName ,
          Address ,
          Route ,
          AdmissionTicket ,
          PlayTime ,
          CarryGoods ,
          Mood ,
          RecommendationIndex ,
          VisitorsNumber ,
          Remark
        )
VALUES  ( @Id ,
          @ScenicSpotName ,
          @Address ,
          @Route ,
          @AdmissionTicket ,
          @PlayTime ,
          @CarryGoods ,
          @Mood ,
          @RecommendationIndex ,
          @VisitorsNumber ,
          @Remark
        )
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@ScenicSpotName", model.ScenicSpotName);
                command.Parameters.AddWithValue("@Address", model.Address);
                command.Parameters.AddWithValue("@Route", model.Route);
                command.Parameters.AddWithValue("@AdmissionTicket", model.AdmissionTicket);
                command.Parameters.AddWithValue("@PlayTime", model.PlayTime);
                command.Parameters.AddWithValue("@CarryGoods", model.CarryGoods);
                command.Parameters.AddWithValue("@Mood", model.Mood);
                command.Parameters.AddWithValue("@RecommendationIndex", model.RecommendationIndex);
                command.Parameters.AddWithValue("@VisitorsNumber", model.VisitorsNumber);
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
DELETE dbo.PB_ScenicSpot WHERE Id=@Id
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("Id", Id);
                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public bool Edit(ScenicSpot model)
        {
            StringBuilder sql = new StringBuilder(@"
UPDATE  dbo.PB_ScenicSpot
SET     ScenicSpotName = @ScenicSpotName ,
        Address = @Address ,
        Route = @Route ,
        AdmissionTicket = @AdmissionTicket ,
        PlayTime = @PlayTime ,
        CarryGoods = @CarryGoods ,
        Mood = @Mood ,
        RecommendationIndex = @RecommendationIndex ,
        VisitorsNumber = @VisitorsNumber ,
        Remark = @Remark ,
        UpdateTime = @UpdateTime
WHERE   Id = @Id
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@ScenicSpotName", model.ScenicSpotName);
                command.Parameters.AddWithValue("@Address", model.Address);
                command.Parameters.AddWithValue("@Route", model.Route);
                command.Parameters.AddWithValue("@AdmissionTicket", model.AdmissionTicket);
                command.Parameters.AddWithValue("@PlayTime", model.PlayTime);
                command.Parameters.AddWithValue("@CarryGoods", model.CarryGoods);
                command.Parameters.AddWithValue("@Mood", model.Mood);
                command.Parameters.AddWithValue("@RecommendationIndex", model.RecommendationIndex);
                command.Parameters.AddWithValue("@VisitorsNumber", model.VisitorsNumber);
                command.Parameters.AddWithNullableValue("@Remark", model.Remark);
                command.Parameters.AddWithValue("UpdateTime", model.UpdateTime);
                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) > 0;
                }
            }
        }

        public List<ScenicSpot> SearchList(SearchScenicSpot condition)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT  *
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY CreateTime DESC ) AS RowNo ,
                    Id ,
                    ScenicSpotName ,
                    Address ,
                    Route ,
                    AdmissionTicket ,
                    PlayTime ,
                    CarryGoods ,
                    Mood ,
                    RecommendationIndex ,
                    VisitorsNumber ,
                    CreateTime ,
                    UpdateTime ,
                    Remark
          FROM      dbo.PB_ScenicSpot
        ) tb
WHERE   tb.RowNo BETWEEN STR(( @PageIndex - 1 ) * @PageSize)
                 AND     STR(@PageIndex * @PageSize)
");
            var list = new List<ScenicSpot>();
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@PageIndex", condition.PageIndex);
                command.Parameters.AddWithValue("@PageSize", condition.PageSize);
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        while (reader.Read())
                            list.Add(new ScenicSpot
                            {

                            });
                    }
                }
            }
            return list;
        }

        public int SearchListMany(SearchScenicSpot condition)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT COUNT(0) Total FROM dbo.PB_ScenicSpot 
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
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
