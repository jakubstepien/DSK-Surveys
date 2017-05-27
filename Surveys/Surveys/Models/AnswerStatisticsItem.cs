using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Models
{
    public class AnswerStatisticsItem
    {
        public Guid IdAnswer { get; set; }

        public int Votes { get; set; }

        public string Name { get; set; }

        public string Text { get { return Name + " - " + Votes; } }
    }
}
