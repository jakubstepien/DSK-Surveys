using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Data.Entities
{
    class CalculatedResult
    {
        [Key]
        public Guid IdCalculatedResult { get; set; }

        [Index]
        public Guid IdSurvey { get; set; }

        [Index]
        [MaxLength(100)]
        public string ClientIdentifier { get; set; }

        public string Result { get; set; }
    }
}
