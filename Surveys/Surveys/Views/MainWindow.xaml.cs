using Surveys.Models;
using Surveys.WCFServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        Guid? currentSurveyId;
        SurveyExchangeService serv = new SurveyExchangeService();
        public ISurveyExchangeService Channel => serv.Channel;

        public MainWindow()
        {
            //Wrzucanie danych do pustej bazy
            //InsertTestData();
            //return;
            serv.StartService();
            Closing += (sender, e) =>
            {
                serv.StopService();
            };

            InitializeComponent();
            surveyView.Visibility = Visibility.Collapsed;
            surveyListView.MainWindow = this;
            surveyView.MainWindow = this;
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

        ///dane wrzucane do bazy zeby byly jakies ankiety zanim sie zrobi doawania ankiet
        private void InsertTestData()
        {
            Guid a1oa = Guid.NewGuid();
            Guid a1ob = Guid.NewGuid();
            Guid a1oc = Guid.NewGuid();
            Guid a2oa = Guid.NewGuid();
            Guid a2ob = Guid.NewGuid();
            Guid a2oc = Guid.NewGuid();
            var list = new List<SurveyModel>
            {
                new SurveyModel
                {
                    IdSurvey = Guid.NewGuid(),
                    Name = "ankieta 1",
                    Description ="Peirwsza ankieta",
                    EndDateUTC = DateTime.Now.AddMonths(10).ToUniversalTime(),
                    Answers = new List<AnswerModel>
                    {
                        new AnswerModel {IdAnswer = a1oa,  Text = "odpowiedz aaa", Votes = 2},
                        new AnswerModel {IdAnswer = a1ob,  Text = "odpowiedz bb", Votes = 5},
                        new AnswerModel {IdAnswer = a1oc,  Text = "odpowiedz c", Votes = 6}
                    }
                },
                new SurveyModel
                {
                    IdSurvey = Guid.NewGuid(),
                    Name = "ankieta 2",
                    Description ="ankieta najlepsza",
                    EndDateUTC = DateTime.Now.AddMonths(10).ToUniversalTime(),
                    Answers = new List<AnswerModel>
                    {
                        new AnswerModel {IdAnswer = a2oa,  Text = "odp aaa", Votes = 4},
                        new AnswerModel {IdAnswer = a2ob,  Text = "odp bb", Votes = 5},
                        new AnswerModel {IdAnswer = a2oc,  Text = "odp c", Votes = 10}
                    }
                }
            };
            var votes = Enumerable.Range(0, 2).Select(s => new VoteModel { IdVote = Guid.NewGuid(), IdAnswer = a1oa })
                        .Concat(Enumerable.Range(0, 5).Select(s => new VoteModel { IdVote = Guid.NewGuid(), IdAnswer = a1ob }))
                        .Concat(Enumerable.Range(0, 6).Select(s => new VoteModel { IdVote = Guid.NewGuid(), IdAnswer = a1oc }))
                        .Concat(Enumerable.Range(0, 4).Select(s => new VoteModel { IdVote = Guid.NewGuid(), IdAnswer = a2oa }))
                        .Concat(Enumerable.Range(0, 5).Select(s => new VoteModel { IdVote = Guid.NewGuid(), IdAnswer = a2ob }))
                        .Concat(Enumerable.Range(0, 10).Select(s => new VoteModel { IdVote = Guid.NewGuid(), IdAnswer = a2oc }));
            var service = new Services.SurveyService();
            list.ForEach(f => service.AddSurvey(f));
            votes.ForEach(f => service.AddVote(f));
        }
     
    }
}
