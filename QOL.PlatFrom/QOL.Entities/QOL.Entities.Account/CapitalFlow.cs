using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Entities.Account
{
    public class CapitalFlow
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Category { get; set; }
        public string AccountType { get; set; }
        public float OutMoney { get; set; }
        public DateTime OrderTime { get; set; }
        public string Consumer { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
