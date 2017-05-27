using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Models
{
    class StatiscticModel
    {
        public string Text { get; set; }

        public int Votes { get; set; }

        public string DisplayText { get { return Text + ": " + Votes; } }

        public Guid AnswerId { get; set; }
    }
}
