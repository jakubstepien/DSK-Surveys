using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Data.Entities
{
    class Answer
    {
        [Key]
        public Guid IdAnswer { get; set; }

        [Index]
        public Guid IdSurvey { get; set; }

        [MaxLength(255)]
        public string Text { get; set; }
    }
}
