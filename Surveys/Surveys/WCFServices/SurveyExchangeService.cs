using Surveys.Utils;
using Surveys.WCFServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Surveys.WCFServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SurveyExchangeService : ISurveyExchangeService
    {
        #region ChannelFields
        public ISurveyExchangeService Channel { get; private set; }
        ServiceHost host = null;
        ChannelFactory<ISurveyExchangeService> channelFactory = null;
        #endregion

        private Guid hostId = Guid.NewGuid();
        private HashSet<Guid> knownHosts = new HashSet<Guid>();
        public int HostNumber { get { return knownHosts.Count; } }

        public SurveyExchangeService()
        {
        }

        #region ChannelMethods
        public void StartService()
        {
            host = new ServiceHost(this);
            host.CloseTimeout = TimeSpan.FromMilliseconds(1);
            host.Open(TimeSpan.FromDays(100));

            
            channelFactory = new ChannelFactory<ISurveyExchangeService>("SurveyExchangeServiceEndpoint");
            Channel = channelFactory.CreateChannel();
            System.Diagnostics.Debug.WriteLine(DateTime.Now);

            //żaden event na host ani channel nie zwraca od kiedy service działa
            Thread.Sleep(2000);
            Channel.Join(hostId);
        }

        public void StopService()
        {
            if (host != null)
            {
                Channel.Exit(hostId);
                if (host.State != CommunicationState.Closed)
                {
                    channelFactory.Close();
                    host.Close();
                }
            }
        }
        #endregion


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

        public void Join(Guid id)
        {

            System.Diagnostics.Debug.WriteLine(DateTime.Now);
            knownHosts.Add(id);
            Channel.Greet(new DirectedContract<Guid> { Target = id, Data = hostId });
        }

        public void Greet(DirectedContract<Guid> greeting)
        {
            //Join przeważnie nie zostaje wyłapany przez inne hosty a great już tak więc słuchamy nawet na te nie do tego klienta
            if (!knownHosts.Contains(greeting.Data))
            {
                Channel.Greet(new DirectedContract<Guid> { Data = hostId, Target = greeting.Data });
                knownHosts.Add(greeting.Data);
            }

        }

        public void Exit(Guid id)
        {
            knownHosts.Remove(id);
        }


    }
}
