using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolioSite.DataModel
{
    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}