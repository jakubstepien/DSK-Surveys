using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Models
{
    public class SurveyModel
    {
        public Guid IdSurvey { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        private DateTime endDateUTC;
        public DateTime EndDateUTC
        {
            get { return endDateUTC; }
            set
            {
                //EF zwraca unspecified który jest local
                if(value.Kind == DateTimeKind.Unspecified || value.Kind == DateTimeKind.Local)
                {
                    value = value.ToUniversalTime();
                }
                endDateUTC = value;
            }
        }

        public IEnumerable<AnswerModel> Answers { get; set; }
    }
}
