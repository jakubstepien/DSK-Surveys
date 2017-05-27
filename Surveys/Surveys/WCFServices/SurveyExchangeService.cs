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
            service.AddVote(new Models.VoteModel { IdVote = msg.IdVote, IdAnswer = msg.IdAnswer });
        }

        public void StartService()
        {
            //Instantiate new ServiceHost
            host = new ServiceHost(this);
            //Open ServiceHost
            host.Open();
            //Create a ChannelFactory and load the configuration setting
            channelFactory = new ChannelFactory<ISurveyExchangeService>("SurveyExchangeServiceEndpoint");
            Channel = channelFactory.CreateChannel();
            //Lets others know that someone new has joined
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
    }
}
