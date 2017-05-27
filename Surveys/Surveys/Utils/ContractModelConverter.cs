using Surveys.Models;
using Surveys.WCFServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Utils
{
    static class ContractModelConverter
    {
        public static VoteModel GetModel(this VoteContract contract)
        {
            return new VoteModel { IdVote = contract.IdVote, IdAnswer = contract.IdAnswer };
        }

        public static VoteContract GetContract(this VoteModel model)
        {
            return new VoteContract { IdVote = model.IdVote, IdAnswer = model.IdAnswer };
        }

        public static SurveyModel GetModel(this SurveyContract contract)
        {
            return new SurveyModel
            {
                IdSurvey = contract.IdSurvey,
                Name = contract.Name,
                Description = contract.Description,
                EndDateUTC = contract.EndDateUTC,
                Answers = contract.Answers.Select(s => new AnswerModel
                {
                    IdAnswer = s.IdAnswer,
                    IdSurvey = s.IdSurvey,
                    Text = s.Text
                }).ToArray()
            };
        }

        public static SurveyContract GetContract(this SurveyModel contract)
        {
            return new SurveyContract
            {
                IdSurvey = contract.IdSurvey,
                Name = contract.Name,
                Description = contract.Description,
                EndDateUTC = contract.EndDateUTC,
                Answers = contract.Answers.Select(s => new AnswerContract
                {
                    IdAnswer = s.IdAnswer,
                    IdSurvey = s.IdSurvey,
                    Text = s.Text
                }).ToArray()
            };
        }
    }
}
