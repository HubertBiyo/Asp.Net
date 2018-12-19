using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Entities.Account
{
    public class ScenicSpot
    {
        public Guid Id { get; set; }
        public string ScenicSpotName { get; set; }
        public string Address { get; set; }
        public string Route { get; set; }
        public float AdmissionTicket { get; set; }
        public DateTime PlayTime { get; set; }
        public string CarryGoods { get; set; }
        public string Mood { get; set; }
        public string RecommendationIndex { get; set; }
        public int VisitorsNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string Remark { get; set; }
    }
}
