using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Models
{
    public class VoteModel
    {
        public Guid IdVote { get; set; }

        public Guid IdAnswer { get; set; }

        public Guid IdSurvey { get; set; }
    }
}
