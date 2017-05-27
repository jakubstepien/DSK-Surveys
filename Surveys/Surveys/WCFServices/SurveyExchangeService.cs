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

        public void Say(string msg)
        {
            Console.WriteLine(msg);
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
            Channel.Say("Admin");
        }
        public void StopService()
        {
            if (host != null)
            {
                Channel.Say("Admin");
                if (host.State != CommunicationState.Closed)
                {
                    channelFactory.Close();
                    host.Close();
                }
            }
        }
    }
}
