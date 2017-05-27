using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Data.Entities;

namespace Surveys.Services
{
    static class Logger
    {

        public static void Log(Exception e)
        {
            Log(LogLevel.Error, e.Message, e.ToString());
        }

        public static void Log(LogLevel level, string message, string detail)
        {
            using(var db = new Data.SurveyDbContext())
            {
                db.ErrorLog.Add(new ErrorLog {LogLevel = level, Detail = detail, Message = message });
                db.SaveChanges();
            }
        }
    }
}
