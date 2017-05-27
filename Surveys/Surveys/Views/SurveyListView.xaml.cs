using Surveys.Models;
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
