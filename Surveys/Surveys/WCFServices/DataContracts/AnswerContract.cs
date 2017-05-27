using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices.DataContracts
{
    [DataContract]
    public class AnswerContract
    {
        [DataMember]
        public Guid IdAnswer { get; set; }

        [DataMember]
        public Guid IdSurvey { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}