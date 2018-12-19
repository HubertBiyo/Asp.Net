using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOL.Entities.Account
{
    public class SearchCondition
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
    }

    public class SearchHuman : SearchCondition
    {

    }
    public class SearchTrainInformation : SearchCondition
    {

    }

    public class SearchCapitalFlow:SearchCondition
    {

    }

    public class SearchScenicSpot:SearchCondition
    {

    }
}
