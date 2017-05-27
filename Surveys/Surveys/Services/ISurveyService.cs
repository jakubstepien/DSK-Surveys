using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Models;
using Surveys.WCFServices.DataContracts;

namespace Surveys.Services
{
    interface ISurveyService
    {
        bool AddVote(VoteModel vote, bool myVote = false);

        bool AddSurvey(SurveyModel survey);

        IEnumerable<SurveyListItem> GetSurveys();

        IEnumerable<AnswerModel> GetAnswers(Guid surveyId);

        SurveyModel GetSurvey(Guid id);
    }
}
