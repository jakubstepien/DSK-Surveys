using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Models;

namespace Surveys.Services
{
    public class InMemorySurveyService : ISurveyService
    {
        static readonly List<SurveyModel> list;

        static InMemorySurveyService()
        {
            list = new List<SurveyModel>
            {
                new SurveyModel
                {
                    IdSurvey = Guid.NewGuid(),
                    Name = "ankieta 1",
                    Description ="Peirwsza ankieta",
                    EndDateUTC = DateTime.Now.AddMonths(10).ToUniversalTime(),
                    Answers = new List<AnswerModel>
                    {
                        new AnswerModel {IdAnswer = Guid.NewGuid(),  Text = "odpowiedz aaa", Votes = 10},
                        new AnswerModel {IdAnswer = Guid.NewGuid(),  Text = "odpowiedz bb", Votes = 14},
                        new AnswerModel {IdAnswer = Guid.NewGuid(),  Text = "odpowiedz c", Votes = 4}
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
                        new AnswerModel {IdAnswer = Guid.NewGuid(),  Text = "odp aaa", Votes = 53},
                        new AnswerModel {IdAnswer = Guid.NewGuid(),  Text = "odp bb", Votes = 6},
                        new AnswerModel {IdAnswer = Guid.NewGuid(),  Text = "odp c", Votes = 17}
                    }
                }
            };
        }

        public InMemorySurveyService()
        {

        }

        public bool AddSurvey(SurveyModel survey)
        {
            list.Add(survey);
            return true;
        }

        public SurveyModel GetSurvey(Guid id)
        {
            return list.SingleOrDefault(s => s.IdSurvey == id);
        }

        public IEnumerable<SurveyListItem> GetSurveys()
        {
            return list.Select(s => new SurveyListItem { IdSurvey = s.IdSurvey, Name = s.Name }).OrderBy(o => o.Name).ToArray();
        }
    }
}
