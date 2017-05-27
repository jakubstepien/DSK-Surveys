using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.WCFServices.DataContracts
{
    [DataContract]
    public class DirectedContract
    {
        [DataMember]
        public Guid Target { get; set; }
    }

    [DataContract]
    public class DirectedContract<T>
    {
        [DataMember]
        public Guid Target { get; set; }

        [DataMember]
        public T Data { get; set; }
    }
}
