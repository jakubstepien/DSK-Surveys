using Surveys.Models;
using Surveys.Services;
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
                try
                {
                    var id = Guid.NewGuid();
                    var slectedDate = surveyEndDate.SelectedDate.Value;
                    var endDate = new DateTime(slectedDate.Year, slectedDate.Month, slectedDate.Day, int.Parse(surveyEndHour.Text), int.Parse(surveyEndMin.Text), 0);
                    var answers = (DataContext as ViewModels.AddSurveyViewModel).Items.Select(s => new AnswerModel { IdSurvey = id, IdAnswer = Guid.NewGuid(), Text = s.Text, Votes = 0 }).ToArray();
                    AddedSurvey = new SurveyModel
                    {
                        IdSurvey = Guid.NewGuid(),
                        Description = surveyDescription.Text,
                        Name = surveyName.Text,
                        EndDateUTC = endDate.ToUniversalTime(),
                        Answers = answers
                    };
                    this.Close();
                    return;
                }
                catch(Exception ex)
                {
                    Logger.Log(ex);
                }
               
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

            if (!surveyEndDate.SelectedDate.HasValue)
                return false;

            return true;
        }

        private void HourValidation(object sender, TextCompositionEventArgs e)
        {
            ValidateTextboxIntRange(0, 23, e);
        }

        private void MinValidation(object sender, TextCompositionEventArgs e)
        {
            ValidateTextboxIntRange(0, 59, e);
        }

        private static void ValidateTextboxIntRange(int min, int max, TextCompositionEventArgs e)
        {
            int minutes = 0;
            if (int.TryParse(e.Text, out minutes))
            {
                e.Handled = minutes >= min && minutes <= max;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}
