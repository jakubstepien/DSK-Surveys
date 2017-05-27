using Surveys.Models;
using Surveys.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for SurveyListView.xaml
    /// </summary>
    public partial class SurveyListView : UserControl
    {
        public IMainWindow MainWindow { get; set; }

        public SurveyListView()
        {
            InitializeComponent();
            LoadSurveys();
            DispatcherTimer refreshListTimer = new DispatcherTimer();
            refreshListTimer.Interval = TimeSpan.FromSeconds(30);
            refreshListTimer.Tick += RefreshListTimer_Tick;
            refreshListTimer.Start();
        }

        private void RefreshListTimer_Tick(object sender, EventArgs e)
        {
            LoadSurveys();
        }

        public void LoadSurveys()
        {
            var surveyService = new Services.SurveyService();
            var items = surveyService.GetSurveys();
            surveys.Items.Clear();
            items.ForEach(f => surveys.Items.Add(f));
        }

        private void AddSurveyClick(object sender, RoutedEventArgs e)
        {
            var window = new AddSurveyWindow();
            window.Closed += (modelCloseSender, modalEvent) =>
            {
                if (window.AddedSurvey != null)
                {
                    MainWindow.Channel.AddSurvey(window.AddedSurvey.GetContract());
                    surveys.Items.Add(new SurveyListItem { IdSurvey = window.AddedSurvey.IdSurvey, Name = window.AddedSurvey.Name });
                }
            };
            window.ShowDialog();
        }

        private void SurveysSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //można zanaczyć tylko jeden więc wystarczy brac pierwszy
            var selected = e.AddedItems.OfType<SurveyListItem>().FirstOrDefault();
            if (selected != null)
            {
                MainWindow.LoadSurvey(selected.IdSurvey);
            }
        }
    }
}
