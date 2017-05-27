using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Data.Entities
{
    class Answer
    {
        [Key]
        public Guid IdAnswer { get; set; }

        public Guid IdSurvey { get; set; }

        [MaxLength(255)]
        public string Text { get; set; }
    }
}
