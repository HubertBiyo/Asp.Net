using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Entities.Account
{
    public class User
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid HumanId { get; set; }
        public string HumanName { get; set; }
        public string PassWord { get; set; }
        public DateTime ValidDate { get; set; }
        public int Actived { get; set; }
        public string ITAdminFlag { get; set; }
        public string IsWinAccount { get; set; }
        public Guid LastEpsProjId { get; set;}
    }
}
