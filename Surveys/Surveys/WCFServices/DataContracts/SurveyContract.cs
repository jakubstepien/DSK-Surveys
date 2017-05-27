using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices.DataContracts
{
    [DataContract]
    public class SurveyContract
    {
        [DataMember]
        public Guid IdSurvey { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime EndDateUTC { get; set; }

        [DataMember]
        public AnswerContract[] Answers { get; set; }
    }
}
