using Surveys.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Surveys.Views
{
    /// <summary>
    /// Interaction logic for AddSurveyWindow.xaml
    /// </summary>
    public partial class AddSurveyWindow : Window
    {
        public SurveyModel AddedSurvey { get; set; }


        public AddSurveyWindow()
        {
            DataContext = new ViewModels.AddSurveyViewModel();
            InitializeComponent();
        }

        private void SaveSurvey(object sender, RoutedEventArgs e)
        {
            if (CanAdd())
            {
                var id = Guid.NewGuid();
                var answers = (DataContext as ViewModels.AddSurveyViewModel).Items.Select(s => new AnswerModel { IdSurvey = id, IdAnswer = Guid.NewGuid(), Text = s.Text, Votes = 0 }).ToArray();
                AddedSurvey = new SurveyModel
                {
                    IdSurvey = Guid.NewGuid(),
                    Description = surveyDescription.Text,
                    Name = surveyName.Text,
                    EndDateUTC = DateTime.Now.AddYears(10),
                    Answers = answers
                };
                var service = new Services.SurveyService();

                //TODO jezeli sie nie doda
                service.AddSurvey(AddedSurvey);

                this.Close();
            }
            else
            {
                //TODO
            }
        }

        private bool CanAdd()
        {
            if (string.IsNullOrWhiteSpace(surveyName.Text))
                return false;

            if (string.IsNullOrWhiteSpace(surveyDescription.Text))
                return false;

            if ((DataContext as ViewModels.AddSurveyViewModel).Items.Count == 0)
                return false;

            return true;
        }
    }
}
