using Surveys.Models;
using Surveys.Services;
using Surveys.WCFServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Surveys.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        Guid? currentSurveyId;
        SurveyExchangeService serv = new SurveyExchangeService();
        public ISurveyExchangeService Channel => serv.Channel;
        private static Mutex _mutex = null;

        public MainWindow()
        {
            if (!CanRun())
            {
                Application.Current.Shutdown();
            }
            else
            {
                this.Title = "DKS - Ankiety client: " + App.ClientIdentifier;
                CultureInfo culture = new CultureInfo("pl-PL");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;

                serv.StartService();

                InitializeComponent();
                surveyView.Visibility = Visibility.Collapsed;
                surveyListView.MainWindow = this;
                surveyView.MainWindow = this;
                DispatcherTimer hostsInfoRefresh = new DispatcherTimer();
                hostsInfoRefresh.Interval = TimeSpan.FromSeconds(5);
                hostsInfoRefresh.Tick += HostsInfoRefresh_Tick;
                hostsInfoRefresh.Start();

                Timer resultsTimer = new Timer(CallculateResults, null, 0, 10000);
                Timer pingingTimer = new Timer(Ping, null, 10000, 2000);
                Timer peers = new Timer(Peers, null, 1000, 10000);
                Closing += (sender, e) =>
                {
                    serv.StopService();
                    resultsTimer.Dispose();
                    pingingTimer.Dispose();
                    peers.Dispose();
                };
            }
        }

        private void Peers(object state)
        {
            if(serv.Peer != null)
            {
                var myRes = new System.Net.PeerToPeer.PeerNameResolver();
                var recColl = myRes.Resolve(serv.Peer);
                Console.WriteLine("Loose Rec Collection " + recColl.Count.ToString());

                foreach (var record in recColl)
                {
                    Console.WriteLine("For peer named : " + record.PeerName);

                    foreach (var endP in record.EndPointCollection)
                    {
                        Console.WriteLine("We have the following endpoint information logged : " + endP.ToString());
                    }

                    Console.WriteLine("----------------------------------------------------------------------------------");
                }
            }
           
        }

        private bool CanRun()
        {
            return true;
            const string appName = "DSK - ankiety";
            bool createdNew;
            _mutex = new Mutex(true, appName, out createdNew);
            return createdNew;
        }

        private void Ping(object state)
        {
            try
            {
                Channel.Ping(App.AppId);
            }
            catch(ObjectDisposedException e) { }
        }

        private async void CallculateResults(object oStateObject)
        {
            var service = new SurveyService();
            var ids = await service.GetSurveysNeedingCalculating();
            foreach(var id in ids)
            {
                var result = await service.CalculateResult(id);
                Channel.SurveyResult(result);
            }
        }

        private void HostsInfoRefresh_Tick(object sender, EventArgs e)
        {
            hostsInfo.Content = "Total hosts: " + serv.HostNumber;
        }

        public void HandleVoted(Guid answerId)
        {
            if (currentSurveyId.HasValue)
            {
                answersStatisticsView.AddVote(answerId);
            }
        }

        public void LoadSurvey(Guid id)
        {
            currentSurveyId = id;
            var service = new Services.SurveyService();
            var survey = service.GetSurvey(id);
            surveyView.LoadSurvey(survey);
            surveyView.Visibility = Visibility.Visible;
            answersStatisticsView.LoadSurvey(survey);
        }
     
    }
}
