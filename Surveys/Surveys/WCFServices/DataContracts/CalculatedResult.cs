using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices.DataContracts
{
    [DataContract]
    public class CalculatedResult
    {
        [DataMember]
        public Guid IdApp { get; set; }

        [DataMember]
        public Guid IdSurvey { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public Result[] Result { get; set; }
    }

    [DataContract]
    public class Result
    {
        [DataMember]
        public Guid IdAnswer { get; set; }

        [DataMember]
        public int Votes { get; set; }
    }
}
