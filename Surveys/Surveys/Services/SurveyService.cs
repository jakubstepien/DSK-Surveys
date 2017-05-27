using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Models;
using Surveys.Data;

namespace Surveys.Services
{
    class SurveyService : ISurveyService
    {
        public bool AddSurvey(SurveyModel survey)
        {
            try
            {
                using (var db = new SurveyDbContext())
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
            }
            catch (Exception e)
            {
                Logger.Log(e);
                return false;
            }
        }

        public bool AddVote(VoteModel vote, bool myVote = false)
        {
            try
            {
                using (var db = new SurveyDbContext())
                {
                    db.Vote.Add(new Data.Entities.Vote { IdVote = vote.IdVote, IdAnswer = vote.IdAnswer });
                    if (myVote)
                    {
                        db.MyVotes.Add(new Data.Entities.MyVotes { IdAnswer = vote.IdAnswer, IdSurvey = vote.IdSurvey });
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Log(e);
                return false;
            }
        }

        public SurveyModel GetSurvey(Guid id)
        {
            using (var db = new SurveyDbContext())
            {
                var surveys = db.Survey
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
                var survey = new SurveyModel
                {
                    IdSurvey = queryReuslt[0].IdSurv,
                    Name = queryReuslt[0].SurvName,
                    Description = queryReuslt[0].SurvDescr,
                    EndDateUTC = queryReuslt[0].SurvEndDate,
                    Answers = queryReuslt.Select(s => new AnswerModel { IdAnswer = s.AnswerModel.IdAnswer, Text = s.AnswerModel.Text, Votes = s.Votes, IdSurvey = s.IdSurv })
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
