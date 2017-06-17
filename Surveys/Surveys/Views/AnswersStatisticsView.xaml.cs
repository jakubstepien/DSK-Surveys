using Surveys.Models;
using Surveys.WCFServices.DataContracts;
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
        DateTime? endDate = null;

        public AnswersStatisticsView()
        {
            InitializeComponent();
            DispatcherTimer answersReloadTimer = new DispatcherTimer();
            answersReloadTimer.Interval = TimeSpan.FromSeconds(10);
            answersReloadTimer.Tick += AnswersReloadTimer_Tick;
            answersReloadTimer.Start();
        }

        private async void AnswersReloadTimer_Tick(object sender, EventArgs e)
        {
            await ReloadResults();
        }

        public void LoadSurvey(SurveyModel survey)
        {
            surveyId = survey.IdSurvey;
            endDate = survey.EndDateUTC;
            calculatedResults.Text = string.Empty;
            statisticsList.Items.Clear();
            var answers = survey.Answers.OrderByDescending(o => o.Votes).Select(s => new StatiscticModel { Text = s.Text, Votes = s.Votes, AnswerId = s.IdAnswer });
            answers.ForEach(f => statisticsList.Items.Add(f));
            GetTotalResults(answers);
        }

        private void GetTotalResults(IEnumerable<StatiscticModel> answers)
        {
            var answersMap = answers.ToDictionary(a => a.AnswerId);
            if (endDate < DateTime.UtcNow)
            {
                var calculatedResult = new Services.SurveyService().GetResults(surveyId.Value);
                var intro = AreResultsSame(calculatedResult) ? "Wszystkie wezły zwróciły ten sam wynik" : "Węzły zwróciły różne wyniki";
                var text = calculatedResult.Select(s => s.ClientId + ":" + Environment.NewLine + string.Join(Environment.NewLine + "\t", s.Result.Select(r => answersMap[r.IdAnswer].Text + " - " + r.Votes)));
                calculatedResults.Text = intro + Environment.NewLine + string.Join(Environment.NewLine, text);
            }
        }

        private bool AreResultsSame(CalculatedResult[] results)
        {
            try
            {
                if (results.Length == 0)
                {
                    return true;
                }
                Dictionary<Guid, int> answerVotes = results[0].Result.ToDictionary(r => r.IdAnswer, r => r.Votes);
                for (int i = 1; i < results.Length; i++)
                {
                    for (int j = 0; j < results[i].Result.Length; j++)
                    {
                        var currentResult = results[i].Result[j];
                        if (answerVotes[currentResult.IdAnswer] != currentResult.Votes)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task ReloadResults()
        {
            if (surveyId.HasValue)
            {
                statisticsList.Items.Clear();
                var service = new Services.SurveyService();
                var answers = await service.GetAnswers(surveyId.Value);
                answers.ForEach(f => statisticsList.Items.Add(new StatiscticModel { Text = f.Text, Votes = f.Votes, AnswerId = f.IdAnswer }));
                statisticsList.Items.Refresh();
                GetTotalResults(statisticsList.Items.OfType<StatiscticModel>());
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
