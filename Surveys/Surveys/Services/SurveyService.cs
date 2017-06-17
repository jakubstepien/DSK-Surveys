using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Models;
using Surveys.Data;
using Surveys.WCFServices.DataContracts;

namespace Surveys.Services
{
    class SurveyService : ISurveyService
    {
        public bool AddSurvey(SurveyContract survey)
        {
            try
            {
                using (var db = new SurveyDbContext())
                {
                    if (!db.Survey.Any(a => a.IdSurvey == survey.IdSurvey))
                    {
                        db.Survey.Add(new Data.Entities.Survey
                        {
                            IdSurvey = survey.IdSurvey,
                            Description = survey.Description,
                            Name = survey.Name,
                            StartDateUTC = DateTime.Now.ToUniversalTime(),
                            EndDateUTC = survey.EndDateUTC
                        });
                        var answers = survey.Answers.Select(s => new Data.Entities.Answer { IdAnswer = s.IdAnswer, IdSurvey = survey.IdSurvey, Text = s.Text });
                        db.Answer.AddRange(answers);
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e);
                return false;
            }
        }

        public bool AddVote(VoteContract vote)
        {
            try
            {
                using (var db = new SurveyDbContext())
                {
                    if (!db.Vote.Any(a => a.IdVote == vote.IdVote))
                    {
                        db.Vote.Add(new Data.Entities.Vote { IdVote = vote.IdVote, IdAnswer = vote.IdAnswer });
                        if (vote.SenderId == App.AppId)
                        {
                            db.MyVotes.Add(new Data.Entities.MyVotes { IdAnswer = vote.IdAnswer, IdSurvey = vote.IdSurvey });
                        }
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e);
                return false;
            }
        }

        public IEnumerable<AnswerModel> GetAnswers(Guid surveyId)
        {
            using (var db = new SurveyDbContext())
            {
                var answers = db.Answer
                    .AsNoTracking()
                    .Where(w => w.IdSurvey == surveyId)
                    .GroupJoin(db.Vote, l => l.IdAnswer, r => r.IdAnswer, (l, r) => new { Answer = l, Vote = r })
                    .SelectMany(s => s.Vote.DefaultIfEmpty(), (s, votes) => new { Answer = s.Answer, Vote = votes })
                    .GroupBy(g => g.Answer)
                    .Select(s => new AnswerModel
                    {
                        IdSurvey = surveyId,
                        IdAnswer = s.Key.IdAnswer,
                        Text = s.Key.Text,
                        Votes = s.Count(c => c.Vote != null)
                    })
                    .OrderByDescending(o => o.Votes);

                return answers.ToArray();
            }
        }

        public CurrentSurveysContract GetCurrentSurveys(Guid senderId)
        {
            var dateUTC = DateTime.UtcNow;
            using (var db = new SurveyDbContext())
            {
                var surveys = db.Survey
                    .AsNoTracking()
                    .Where(w => w.EndDateUTC > dateUTC)
                    .Join(db.Answer, l => l.IdSurvey, r => r.IdSurvey, (l, r) => new { Survey = l, Answer = r })
                    .GroupBy(g => new { g.Survey })
                    .Select(s => new
                    {
                        IdSurvey = s.Key.Survey.IdSurvey,
                        Name = s.Key.Survey.Name,
                        Description = s.Key.Survey.Description,
                        EndDateUTC = s.Key.Survey.EndDateUTC,

                        Answers = s.Select(a => new AnswerContract { Text = a.Answer.Text, IdAnswer = a.Answer.IdAnswer, IdSurvey = a.Answer.IdAnswer }),
                    }).ToArray();
                var votes = db.Vote.AsNoTracking().Select(s => new { IdAnswer = s.IdAnswer, IdVote = s.IdVote, SenderId = senderId }).ToArray();
                var surveyContracts = surveys.Select(s => new SurveyContract { Answers = s.Answers.ToArray(), Description = s.Description, EndDateUTC = s.EndDateUTC, IdSurvey = s.IdSurvey, Name = s.Name }).ToArray();
                var votesContracts = votes.Select(s => new VoteContract { IdAnswer = s.IdAnswer, IdVote = s.IdVote, SenderId = s.SenderId }).ToArray();
                return new CurrentSurveysContract { Surveys = surveyContracts, Votes = votesContracts };
            }
        }

        public SurveyModel GetSurvey(Guid id)
        {
            using (var db = new SurveyDbContext())
            {
                var surveys = db.Survey
                    .AsNoTracking()
                    .Where(w => w.IdSurvey == id)
                    .Join(db.Answer, l => l.IdSurvey, r => r.IdSurvey, (l, r) => new { Survey = l, Answer = r })
                    .GroupJoin(db.Vote, l => l.Answer.IdAnswer, r => r.IdAnswer, (l, r) => new { Survey = l.Survey, Answer = l.Answer, Vote = r })
                    .SelectMany(s => s.Vote.DefaultIfEmpty(), (s, votes) => new { Survey = s.Survey, Answer = s.Answer, Vote = votes })
                    .GroupBy(g => new { g.Survey, g.Answer })
                    .Select(s => new
                    {
                        IdSurv = s.Key.Survey.IdSurvey,
                        SurvName = s.Key.Survey.Name,
                        SurvDescr = s.Key.Survey.Description,
                        SurvEndDate = s.Key.Survey.EndDateUTC,

                        AnswerModel = s.Key.Answer,
                        Votes = s.Count(a => a.Vote != null)
                    });
                var queryReuslt = surveys.ToList();
                var test = surveys.ToString();

                if (queryReuslt.Count == 0)
                {
                    return null;
                }

                //Po wyłączeniu głosowanie pare razy zmienić na SingleOrDefault
                var selectedAnswer = db.MyVotes.AsNoTracking().Where(s => s.IdSurvey == id).Select(s => s.IdAnswer).FirstOrDefault();

                var survey = new SurveyModel
                {
                    IdSurvey = queryReuslt[0].IdSurv,
                    Name = queryReuslt[0].SurvName,
                    Description = queryReuslt[0].SurvDescr,
                    EndDateUTC = queryReuslt[0].SurvEndDate,
                    Answers = queryReuslt.Select(s => new AnswerModel
                    {
                        IdAnswer = s.AnswerModel.IdAnswer,
                        Text = s.AnswerModel.Text,
                        Votes = s.Votes,
                        IdSurvey = s.IdSurv,
                        IsChecked = s.AnswerModel.IdAnswer == selectedAnswer
                    })
                };
                return survey;
            }
        }

        public IEnumerable<SurveyListItem> GetSurveys()
        {
            using (var db = new SurveyDbContext())
            {

                var surveys = db.Survey.AsNoTracking().Select(s => new SurveyListItem { IdSurvey = s.IdSurvey, Name = s.Name }).ToArray();
                return surveys;
            }
        }
    }
}
