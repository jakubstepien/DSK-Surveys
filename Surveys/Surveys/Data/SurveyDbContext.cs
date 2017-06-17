using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surveys.Data.Entities;
using SQLite.CodeFirst;

namespace Surveys.Data
{
    class SurveyDbContext : DbContext
    {
        public SurveyDbContext() : base("defaultSqlLite")
        {
        }

        #region DbSets

        public DbSet<Survey> Survey { get; set; }

        public DbSet<Answer> Answer { get; set; }

        public DbSet<Vote> Vote { get; set; }

        public DbSet<MyVotes> MyVotes { get; set; }

        public DbSet<ErrorLog> ErrorLog { get; set; }

        public DbSet<CalculatedResult> CalculatedResult { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteDropCreateDatabaseWhenModelChanges<SurveyDbContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);

        }
    }

}

