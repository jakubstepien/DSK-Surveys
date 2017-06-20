using Surveys.Services;
using Surveys.Utils;
using Surveys.WCFServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.PeerToPeer;
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
        public PeerName Peer { get; private set; }
        ServiceHost host = null;
        ChannelFactory<ISurveyExchangeService> channelFactory = null;
        #endregion

        private HashSet<Guid> knownHosts = new HashSet<Guid>();
        public int HostNumber { get { return knownHosts.Count; } }

        public SurveyExchangeService()
        {
        }

        #region ChannelMethods
        public void StartService()
        {

            Peer = new PeerName(App.ClientIdentifier, PeerNameType.Unsecured);
            PeerNameRegistration registeration = new PeerNameRegistration(Peer, 3030);
            registeration.Cloud = Cloud.Available;
            registeration.Comment = App.AppId.ToString();
            Console.WriteLine(string.Join(Environment.NewLine, Cloud.GetAvailableClouds().Select(s => "name: " +  s.Name + " scope: " + s.Scope)));
            registeration.Start();

            host = new ServiceHost(this);
            host.CloseTimeout = TimeSpan.FromMilliseconds(1);
            host.Open(TimeSpan.FromDays(100));


            channelFactory = new ChannelFactory<ISurveyExchangeService>("SurveyExchangeServiceEndpoint");
            Channel = channelFactory.CreateChannel();

            //żaden event na host ani channel nie zwraca od kiedy service działa
            Thread.Sleep(2000);
            Channel.Ping(App.AppId);
            host.Closing += (sender, e) => registeration.Stop();
        }

        public void StopService()
        {
            if (host != null)
            {
                Channel.Exit(App.AppId);
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
            service.AddVote(msg);
        }

        public void AddSurvey(SurveyContract survey)
        {
            var service = new Services.SurveyService();
            service.AddSurvey(survey);
        }

        public void Ping(Guid id)
        {
            if (!knownHosts.Contains(id))
            {
                knownHosts.Add(id);
                Channel.Greet(new DirectedContract<Guid> { Target = id, Data = App.AppId });
                SendCurrentSurveysToNewHost(id);
            }
        }

        public void Greet(DirectedContract<Guid> greeting)
        {
            //Join przeważnie nie zostaje wyłapany przez inne hosty a great już tak więc słuchamy nawet na te nie do tego klienta
            if (!knownHosts.Contains(greeting.Data))
            {
                Channel.Greet(new DirectedContract<Guid> { Data = App.AppId, Target = greeting.Data });
                knownHosts.Add(greeting.Data);
                SendCurrentSurveysToNewHost(greeting.Target);
            }

        }

        private void SendCurrentSurveysToNewHost(Guid targetId)
        {
            if (targetId != App.AppId)
            {
                var service = new SurveyService();
                var currentSurveys = service.GetCurrentSurveys(App.AppId);
                currentSurveys.Target = targetId;
                Channel.CurrentSurveys(currentSurveys);
            }
        }

        public void Exit(Guid id)
        {
            knownHosts.Remove(id);
        }

        public void CurrentSurveys(CurrentSurveysContract surveys)
        {
            if (surveys.Target == App.AppId)
            {
                var service = new Services.SurveyService();
                surveys.Surveys.ForEach(survey =>
                {
                    service.AddSurvey(survey);
                });
                surveys.Votes.ForEach(vote =>
                {
                    service.AddVote(vote);
                });
                surveys.Results.ForEach(result =>
                {
                    service.AddResult(result);
                });
            }
        }

        public void SurveyResult(CalculatedResult result)
        {
            var service = new SurveyService();
            service.AddResult(result);
        }
    }
}
