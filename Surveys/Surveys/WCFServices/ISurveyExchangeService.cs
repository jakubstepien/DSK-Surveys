using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Surveys.WCFServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISurveyExchangeService" in both code and config file together.
    [ServiceContract]
    public interface ISurveyExchangeService
    {
        [OperationContract(IsOneWay = true)]
        void Say(string msg);
    }
}
