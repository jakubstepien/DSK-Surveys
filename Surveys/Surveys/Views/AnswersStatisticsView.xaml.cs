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
    /// Interaction logic for AnswersStatisticsView.xaml
    /// </summary>
    public partial class AnswersStatisticsView : UserControl
    {
        public AnswersStatisticsView()
        {
            InitializeComponent();
        }

        public void LoadSurvey(SurveyModel survey)
        {
            statisticsList.Items.Clear();
            var answers = survey.Answers.OrderByDescending(o => o.Votes).Select(s => new { Text = s.Text + ": " + s.Votes });
            answers.ForEach(f => statisticsList.Items.Add(f));
        }
    }
}
