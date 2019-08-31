using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPortfolioSite.DataModel
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public virtual ICollection<People> Peoples { get; set; }
    }
}