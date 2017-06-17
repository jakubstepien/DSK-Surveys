using Surveys.Models;
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

        public MainWindow()
        {
            //MessageBox.Show(new Services.MacAdressService().GetMacAdress());

            CultureInfo culture = new CultureInfo("pl-PL");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            serv.StartService();
            Closing += (sender, e) =>
            {
                serv.StopService();
            };

            InitializeComponent();
            surveyView.Visibility = Visibility.Collapsed;
            surveyListView.MainWindow = this;
            surveyView.MainWindow = this;

            DispatcherTimer hostsInfoRefresh = new DispatcherTimer();
            hostsInfoRefresh.Interval = TimeSpan.FromSeconds(5);
            hostsInfoRefresh.Tick += HostsInfoRefresh_Tick;
            hostsInfoRefresh.Start();
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
