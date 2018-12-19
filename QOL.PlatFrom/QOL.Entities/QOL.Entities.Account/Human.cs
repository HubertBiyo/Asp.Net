using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Entities.Account
{
    public class Human
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string LocalAddress { get; set; }
        public string OpenId { get; set; }
        public int RoleType { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Status { get; set; }
    }
}
