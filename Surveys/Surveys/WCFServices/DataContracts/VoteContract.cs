using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices.DataContracts
{
    [DataContract]
    public class VoteContract
    {
        [DataMember]
        public Guid IdVote { get; set; }

        [DataMember]
        public Guid IdAnswer { get; set; }

        [DataMember]
        public Guid IdSurvey { get; set; }
    }
}
