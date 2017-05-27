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
using System.Windows.Threading;

namespace Surveys.Views
{
    /// <summary>
    /// Interaction logic for AnswersStatisticsView.xaml
    /// </summary>
    public partial class AnswersStatisticsView : UserControl
    {
        Guid? surveyId = null;

        public AnswersStatisticsView()
        {
            InitializeComponent();
            DispatcherTimer answersReloadTimer = new DispatcherTimer();
            answersReloadTimer.Interval = TimeSpan.FromSeconds(10);
            answersReloadTimer.Tick += AnswersReloadTimer_Tick;
            answersReloadTimer.Start();
        }

        private void AnswersReloadTimer_Tick(object sender, EventArgs e)
        {
            ReloadResults();
        }

        public void LoadSurvey(SurveyModel survey)
        {
            surveyId = survey.IdSurvey;
            statisticsList.Items.Clear();
            var answers = survey.Answers.OrderByDescending(o => o.Votes).Select(s => new StatiscticModel { Text = s.Text, Votes = s.Votes, AnswerId = s.IdAnswer });
            answers.ForEach(f => statisticsList.Items.Add(f));
        }

        public void ReloadResults()
        {
            if (surveyId.HasValue)
            {
                statisticsList.Items.Clear();
                var service = new Services.SurveyService();
                var answers = service.GetAnswers(surveyId.Value);
                answers.ForEach(f => statisticsList.Items.Add(new StatiscticModel { Text = f.Text, Votes = f.Votes, AnswerId = f.IdAnswer }));
                statisticsList.Items.Refresh();
            }
        }

        public void AddVote(Guid answerId)
        {
            IEnumerable<StatiscticModel> items = statisticsList.Items.OfType<StatiscticModel>().ToList();
            var item = items.SingleOrDefault(w => w.AnswerId == answerId);
            if (item != null)
            {
                item.Votes++;
            }
            statisticsList.Items.Clear();
            items = items.OrderByDescending(o => o.Votes);
            items.ForEach(f => statisticsList.Items.Add(f));
            statisticsList.Items.Refresh();
        }
    }
}
