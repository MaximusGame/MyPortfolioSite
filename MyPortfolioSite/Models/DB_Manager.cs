using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyPortfolioSite.Models
{
    public class Context_Model
    {
       public IEnumerable<People> Peoples_Model { get; set; }
       public IEnumerable<Project> Projects_Model { get; set; }
    }

    public class ModelForAddProjectToPeople
    {
        public People People { get; set; }
        public IEnumerable<Project> Projects_Model { get; set; }
    }

    public class DB_Manager: IContext
    {
        private DB_Contex DB_Contex;
        public static int ProjectID { get; set; }

        public DB_Manager()
        {
            if (DB_Contex == null)
            {
                DB_Contex = new DB_Contex();
            }            
        }


        public static int ConvertProjectID(string ProjectID)
        {
            int x = int.Parse(ProjectID);
            return x;
        }

        public List<People> ReturneDB_People()
        {
            return DB_Contex.Peoples.ToList();
        }

        public void AddNewPeople(People people)
        {
            if (people != null)
            {
                DB_Contex.Peoples.Add(new People { Name = people.Name, Phone = people.Phone });
                DB_Contex.SaveChanges();
            }
        }

        public void EditePeople(People people)
        {
            DB_Contex.Entry(people).State = EntityState.Modified;
            DB_Contex.SaveChanges();
        }

        public void DeletePeople(int id)
        {
            People people = ReturnPeople(id);
            DB_Contex.Peoples.Remove(people);
            DB_Contex.SaveChanges();
        }

        public List<Project> ReturnDB_Projects()
        {
            return DB_Contex.Projects.ToList();
        }

        public People ReturnPeople( int? id)
        {
           return DB_Contex.Peoples.Find(id);
        }

        public void AddNewProject(Project project)
        {
            if (project != null)
            {
                DB_Contex.Projects.Add(new Project { Name = project.Name, Price = project.Price });
                DB_Contex.SaveChanges();
            }
        }

        public void PeopleInProject(int? PeopleID)
        {
            Project project = ReturnProject(ProjectID);
            People people = ReturnPeople(PeopleID);
            people.Projects.Add(project);
            DB_Contex.SaveChanges();
        }

        public Project ReturnProject(int? id)
        {
            return DB_Contex.Projects.Find(id);
        }

        public void EditeProject(Project project)
        {
            DB_Contex.Entry(project).State = EntityState.Modified;
            DB_Contex.SaveChanges();
        }

        public void DeleteProject(int id)
        {
            Project project = ReturnProject(id);
            DB_Contex.Projects.Remove(project);
            DB_Contex.SaveChanges();
        }
    }
}