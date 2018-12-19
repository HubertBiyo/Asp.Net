using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Entities.Account
{
    /// <summary>
    /// 日常出行表
    /// </summary>
    public class TrainInformation
    {
        public Guid Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Origin_Time { get; set; }
        public string Destination_Time { get; set; }
        public string TrainNumber { get; set; }
        public string SeatType { get; set; }
        public int Carriage { get; set; }
        public string SeatNumber { get; set; }
        public string TName { get; set; }
        public string TicketType { get; set; }
        public DateTime OrderTime { get; set; }
        public float TicketMoney { get; set; }
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
