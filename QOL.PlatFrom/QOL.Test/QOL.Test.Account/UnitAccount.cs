using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QOL.Entities.Account;
using QOL.Service.Account;

namespace QOL.Test.Account
{
    [TestClass]
    public class UnitAccount
    {
        [TestMethod]

        public void AddHuman()
        {
            var isOk =
            HumanService.Instance.Add( new Human
               {
                   Id = Guid.NewGuid(),
                   Code = "001",
                   Name = "豆旭波",
                   Birthday = Convert.ToDateTime("1995-06-28"),
                   Phone = "15130079715",
                   LocalAddress = "河北省邯郸市永年县",
               });
            Assert.IsNotNull(isOk);
        }
        [TestMethod]
        public void DES()
        {
            var password = QOL.Common.Security.DES.Encrypt3DES("123456");
            Assert.IsNotNull(password);
        }
        [TestMethod]
        public void TestMethod1()
        {
            SearchCapitalFlow condition = new SearchCapitalFlow();
            condition.PageIndex = 1;
            condition.PageSize = 10;
            var result = QOL.Service.Account.CapitalFlowService.Instance.SearchList(condition);
        }
        [TestMethod]
        public void SearchTrainAllList()
        {
            var result = QOL.Service.Account.TrainInformationService.Instance.SearchTrainAllList();
        }
    }
}
