using Surveys.Utils;
using Surveys.WCFServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SurveyExchangeService : ISurveyExchangeService
    {
        public ISurveyExchangeService Channel { get; private set; }
        ServiceHost host = null;
        ChannelFactory<ISurveyExchangeService> channelFactory = null;

        public void Vote(VoteContract msg)
        {
            var service = new Services.SurveyService();
            service.AddVote(msg.GetModel());
        }

        public void AddSurvey(SurveyContract survey)
        {
            var service = new Services.SurveyService();
            service.AddSurvey(survey.GetModel());
        }




        #region ChannelMethods
        public void StartService()
        {
            host = new ServiceHost(this);
            host.Open();
            channelFactory = new ChannelFactory<ISurveyExchangeService>("SurveyExchangeServiceEndpoint");
            Channel = channelFactory.CreateChannel();
        }
        public void StopService()
        {
            if (host != null)
            {
                if (host.State != CommunicationState.Closed)
                {
                    channelFactory.Close();
                    host.Close();
                }
            }
        }
        #endregion
    }
}
