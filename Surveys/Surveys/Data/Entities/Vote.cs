﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Data.Entities
{
    class Vote
    {
        [Key]
        public Guid IdVote { get; set; }

        public Guid IdAnswer { get; set; }
    }
}
