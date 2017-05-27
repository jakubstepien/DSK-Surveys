using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Models
{
    public class SurveyListItem
    {
        public Guid IdSurvey { get; set; }

        public string Name { get; set; }

        public DateTime EndDateUTC { get; set; }

        public List<AnswerModel> Answers { get; set; }
    }
}
