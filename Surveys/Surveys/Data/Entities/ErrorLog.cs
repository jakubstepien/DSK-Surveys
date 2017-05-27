using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveys.Data.Entities
{
    class ErrorLog
    {
        [Key]
        public int Id { get; set; }

        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public string Detail { get; set; }

    }

    enum LogLevel
    {
        Debug, Info, Waring, Error, Critical
    }
}
