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
        void Ping(Guid id);

        [OperationContract(IsOneWay = true)]
        void Greet(DirectedContract<Guid> greeting);

        [OperationContract(IsOneWay = true)]
        void CurrentSurveys(CurrentSurveysContract surveys);

        [OperationContract(IsOneWay = true)]
        void SurveyResult(CalculatedResult result);

        [OperationContract(IsOneWay = true)]
        void Exit(Guid id);

    }
}
