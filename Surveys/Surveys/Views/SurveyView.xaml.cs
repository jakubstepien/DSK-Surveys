﻿using Surveys.Services;
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
    /// Interaction logic for SurveyView.xaml
    /// </summary>
    public partial class SurveyView : UserControl
    {
        public IMainWindow MainWindow { get; set; }
        private DateTime? currentSurveyEnd;
        private Guid surveyId;

        public SurveyView()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (currentSurveyEnd.HasValue)
            {
                string text = "Ankieta zkończona";
                var currTime = DateTime.UtcNow;
                var span = currentSurveyEnd.Value - currTime;
                if (currTime > currentSurveyEnd)
                {
                    currentSurveyEnd = null;
                    sendButton.IsEnabled = false;
                }
                else
                {
                    text = span.Days + "d " + span.Hours + "h " + span.Minutes + "m " + span.Seconds + "s";
                }
                surveyCountDownTimer.Content = text;
            }
        }

        public void LoadSurvey(Models.SurveyModel survey)
        {
            currentSurveyEnd = survey.EndDateUTC;
            surveyName.Content = survey.Name;
            surveyDescription.Content = survey.Description;
            answers.Items.Clear();
            survey.Answers.ForEach(f => answers.Items.Add(f));
            surveyId = survey.IdSurvey;
            sendButton.IsEnabled = !survey.Answers.Any(a => a.IsChecked);
            var currTime = DateTime.UtcNow;
            if (currTime > currentSurveyEnd)
            {
                sendButton.IsEnabled = false;
            }
        }

        private void SendClick(object sender, RoutedEventArgs e)
        {
            var checkedItem = answers.Items.OfType<Models.AnswerModel>().SingleOrDefault(s => s.IsChecked);
            if (checkedItem != null && sendButton.IsEnabled)
            {
                sendButton.IsEnabled = false;
                MainWindow.Channel.Vote(new WCFServices.DataContracts.VoteContract
                {
                    IdVote = Guid.NewGuid(),
                    IdAnswer = checkedItem.IdAnswer,
                    IdSurvey = checkedItem.IdSurvey,
                    SenderId = App.AppId
                });
                MainWindow.HandleVoted(checkedItem.IdAnswer);
            }
        }
    }
}
