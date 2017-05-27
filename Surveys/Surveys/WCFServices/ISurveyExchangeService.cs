using Surveys.WCFServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Surveys.WCFServices
{
    [ServiceContract]
    public interface ISurveyExchangeService
    {
        [OperationContract(IsOneWay = true)]
        void Vote(VoteContract msg);
    }
}
