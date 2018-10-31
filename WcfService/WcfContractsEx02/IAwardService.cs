using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WcfContractsEx02
{
    [ServiceContract(SessionMode =SessionMode.Required)]
    public interface IAwardService
    {
        [OperationContract(IsOneWay =true,IsInitiating =true)]
        void SetPassMark(int passMark);

        [OperationContract]
        Person[] GetAwardedPeople(Person[] peopleToTest);
    }
}
