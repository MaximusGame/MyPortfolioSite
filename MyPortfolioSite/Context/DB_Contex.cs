using MyPortfolioSite.DataModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MyPortfolioSite.Context
{
    public class DB_Contex : DbContext
    {
        public DB_Contex() : base("DB_People")
        {

        }       

        public DbSet<People> Peoples { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
  
}