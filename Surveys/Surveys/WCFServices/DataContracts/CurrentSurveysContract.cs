using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices.DataContracts
{
    [DataContract]
    public class CurrentSurveysContract : DirectedContract
    {
        [DataMember]
        public SurveyContract[] Surveys { get; set; }

        [DataMember]
        public VoteContract[] Votes { get; set; }
    }
}
