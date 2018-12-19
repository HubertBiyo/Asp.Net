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
    public class SqlTrainInformationDataProvider : ITrainInformationDataProvider
    {
        public bool Add(TrainInformation model)
        {
            StringBuilder sql = new StringBuilder(@"
INSERT  dbo.PB_TrainInformation
        ( Id ,
          OrderNo ,
          DepartureTime ,
          Origin_Time ,
          Destination_Time ,
          TrainNumber ,
          SeatType ,
          Carriage ,
          SeatNumber,
          TName ,
          TicketType ,
          OrderTime ,
          TicketMoney ,
          Remark
        )
VALUES  ( @Id ,
          @OrderNo ,
          @DepartureTime ,
          @Origin_Time ,
          @Destination_Time ,
          @TrainNumber ,
          @SeatType ,
          @Carriage ,
          @SeatNumber,
          @TName ,
          @TicketType ,
          @OrderTime ,
          @TicketMoney ,
          @Remark  
        )");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@OrderNo", model.OrderNo);
                command.Parameters.AddWithValue("@DepartureTime", model.DepartureTime);
                command.Parameters.AddWithValue("@Origin_Time", model.Origin_Time);
                command.Parameters.AddWithValue("@Destination_Time", model.Destination_Time);
                command.Parameters.AddWithValue("@TrainNumber", model.TrainNumber);
                command.Parameters.AddWithValue("@SeatType", model.SeatType);
                command.Parameters.AddWithValue("@Carriage", model.Carriage);
                command.Parameters.AddWithValue("@SeatNumber", model.SeatNumber);
                command.Parameters.AddWithValue("@TName", model.TName);
                command.Parameters.AddWithValue("@TicketType", model.TicketType);
                command.Parameters.AddWithValue("@OrderTime", model.OrderTime);
                command.Parameters.AddWithValue("@TicketMoney", model.TicketMoney);
                command.Parameters.AddWithNullableValue("@Remark", model.Remark);

                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public bool Delete(Guid Id)
        {
            StringBuilder sql = new StringBuilder(@"DELETE dbo.PB_TrainInformation WHERE Id=@Id");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", Id);
                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public bool Edit(TrainInformation model)
        {
            StringBuilder sql = new StringBuilder(@"
UPDATE  dbo.PB_TrainInformation
SET     OrderNo = @OrderNo ,
        DepartureTime = @DepartureTime ,
        Origin_Time = @Origin_Time ,
        Destination_Time = @Destination_Time ,
        TrainNumber = @TrainNumber ,
        SeatType = @SeatType ,
        Carriage = @Carriage ,
        SeatNumber = @SeatNumber ,
        TName = @TName ,
        TicketType = @TicketType ,
        OrderTime = @OrderTime ,
        TicketMoney = @TicketMoney ,
        Remark = @Remark
WHERE   Id = @Id
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", model.Id);
                command.Parameters.AddWithValue("@OrderNo", model.OrderNo);
                command.Parameters.AddWithValue("@DepartureTime", model.DepartureTime);
                command.Parameters.AddWithValue("@Origin_Time", model.Origin_Time);
                command.Parameters.AddWithValue("@Destination_Time", model.Destination_Time);
                command.Parameters.AddWithValue("@TrainNumber", model.TrainNumber);
                command.Parameters.AddWithValue("@SeatType", model.SeatType);
                command.Parameters.AddWithValue("@Carriage", model.Carriage);
                command.Parameters.AddWithValue("@SeatNumber", model.SeatNumber);
                command.Parameters.AddWithValue("@TName", model.TName);
                command.Parameters.AddWithValue("@TicketType", model.TicketType);
                command.Parameters.AddWithValue("@OrderTime", model.OrderTime);
                command.Parameters.AddWithValue("@TicketMoney", model.TicketMoney);
                command.Parameters.AddWithNullableValue("@Remark", model.Remark);

                using (var executor = new SqlExecutor())
                {
                    return executor.ExecuteNonQuery(command, DataProviders.MainConnectionString_ReadWrite) == 1;
                }
            }
        }

        public List<TrainInformation> SearchList(SearchTrainInformation condition)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT  *
FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY CreateTime DESC ) AS RowNo ,
                    Id ,
                    OrderNo ,
                    DepartureTime ,
                    Origin_Time ,
                    Destination_Time ,
                    TrainNumber ,
                    SeatType ,
                    Carriage ,
                    SeatNumber,
                    TName ,
                    TicketType ,
                    OrderTime ,
                    TicketMoney ,
                    Remark ,
                    CreateTime
          FROM      dbo.PB_TrainInformation
        ) TB
WHERE   TB.RowNo BETWEEN STR(( @pageIndex - 1 ) * @PageSize + 1)
                 AND     STR(@PageIndex * @PageSize)
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@PageIndex", condition.PageIndex);
                command.Parameters.AddWithValue("@PageSize", condition.PageSize);
                var list = new List<TrainInformation>();
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        while (reader.Read())
                        {
                            list.Add(new TrainInformation
                            {
                                Id = reader.ReadGuid("Id"),
                                OrderNo = reader.ReadString("OrderNo"),
                                DepartureTime = reader.ReadDateTime("DepartureTime"),
                                Origin_Time = reader.ReadString("Origin_Time"),
                                Destination_Time = reader.ReadString("Destination_Time"),
                                TrainNumber = reader.ReadString("TrainNumber"),
                                SeatType = reader.ReadString("SeatType"),
                                Carriage = reader.ReadInt("Carriage"),
                                SeatNumber = reader.ReadString("SeatNumber"),
                                TName = reader.ReadString("TName"),
                                TicketType = reader.ReadString("TicketType"),
                                OrderTime = reader.ReadDateTime("OrderTime"),
                                TicketMoney = reader.ReadFloat("TicketMoney"),
                                Remark = reader.ReadString("Remark"),
                                CreateTime = reader.ReadDateTime("CreateTime"),
                            });
                        }
                    }
                }
                return list;
            }

        }

        public int SearchListMany(SearchTrainInformation condition)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT COUNT(0) Total FROM dbo.PB_TrainInformation
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

        public List<TrainInformation> SearchTrainAllList()
        {
            StringBuilder sql = new StringBuilder(@"
SELECT    ROW_NUMBER() OVER ( ORDER BY CreateTime DESC ) AS RowNo ,
                    Id ,
                    OrderNo ,
                    DepartureTime ,
                    Origin_Time ,
                    Destination_Time ,
                    TrainNumber ,
                    SeatType ,
                    Carriage ,
                    SeatNumber,
                    TName ,
                    TicketType ,
                    OrderTime ,
                    TicketMoney ,
                    Remark ,
                    CreateTime
          FROM      dbo.PB_TrainInformation
");
            var list = new List<TrainInformation>();
            using (var command = new SqlCommand(sql.ToString()))
            {
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        while (reader.Read())
                        {
                            list.Add(new TrainInformation
                            {
                                Id = reader.ReadGuid("Id"),
                                OrderNo = reader.ReadString("OrderNo"),
                                DepartureTime = reader.ReadDateTime("DepartureTime"),
                                Origin_Time = reader.ReadString("Origin_Time"),
                                Destination_Time = reader.ReadString("Destination_Time"),
                                TrainNumber = reader.ReadString("TrainNumber"),
                                SeatType = reader.ReadString("SeatType"),
                                Carriage = reader.ReadInt("Carriage"),
                                SeatNumber = reader.ReadString("SeatNumber"),
                                TName = reader.ReadString("TName"),
                                TicketType = reader.ReadString("TicketType"),
                                OrderTime = reader.ReadDateTime("OrderTime"),
                                TicketMoney = reader.ReadFloat("TicketMoney"),
                                Remark = reader.ReadString("Remark"),
                                CreateTime = reader.ReadDateTime("CreateTime"),
                            });
                        }

                    }
                }
            }
            return list;
        }

        public bool IsHaveDataById(Guid id)
        {
            StringBuilder sql = new StringBuilder(@"
SELECT count(0) Total FROM dbo.PB_TrainInformation WHERE Id=@Id
");
            using (var command = new SqlCommand(sql.ToString()))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var executor = new SqlExecutor())
                {
                    using (var reader = executor.ExecuteReader(command, DataProviders.MainConnectionString_Readonly))
                    {
                        if (reader.Read())
                            return reader.ReadInt("Total") > 0;
                        return false;
                    }
                }
            }
        }
    }
}
