using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Models;

namespace Surveys.Services
{
    interface ISurveyService
    {
        bool AddVote(VoteModel vote, bool myVote = false);

        bool AddSurvey(SurveyModel survey);

        IEnumerable<SurveyListItem> GetSurveys();

        SurveyModel GetSurvey(Guid id);
    }
}
