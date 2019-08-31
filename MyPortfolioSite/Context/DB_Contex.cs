using MyPortfolioSite.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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