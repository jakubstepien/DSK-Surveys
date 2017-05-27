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

        [OperationContract(IsOneWay = true)]
        void AddSurvey(SurveyContract survey);

        [OperationContract(IsOneWay = true)]
        void Join(Guid id);

        /// <summary>
        /// Sends Guid with endpoint id to other endpoint
        /// </summary>
        /// <param name="greeting"></param>
        [OperationContract(IsOneWay = true)]
        void Greet(DirectedContract<Guid> greeting);

        [OperationContract(IsOneWay = true)]
        void Exit(Guid id);
    }
}
