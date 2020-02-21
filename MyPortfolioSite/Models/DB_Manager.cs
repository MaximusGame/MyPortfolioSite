using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


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

    //public class DB_Manager: IContext
    //{
    //    private DB_Contex _DB_Contex;
    //    public static int ProjectID { get; set; }

    //    public DB_Manager(DB_Contex DB_Contex)
    //    {
    //        _DB_Contex = DB_Contex;
    //        //if (DB_Contex == null)
    //        //{
    //        //    _DB_Contex = new DB_Contex();
    //        //}
    //    }


    //    public static int ConvertProjectID(string ProjectID)
    //    {
    //        int x = int.Parse(ProjectID);
    //        return x;
    //    }

    //    public List<People> ReturneDB_People()
    //    {
    //        return _DB_Contex.Peoples.ToList();
    //    }

    //    public void AddNewPeople(People people)
    //    {
    //        if (people != null)
    //        {
    //            _DB_Contex.Peoples.Add(new People { Name = people.Name, Phone = people.Phone, FotoName = people.FotoName});
    //            _DB_Contex.SaveChanges();
    //        }
    //    }

    //    public void EditePeople(People people)
    //    {
    //        _DB_Contex.Entry(people).State = EntityState.Modified;
    //        _DB_Contex.SaveChanges();
    //    }

    //    public void DeletePeople(int id)
    //    {
    //        People people = ReturnPeople(id);
    //        _DB_Contex.Peoples.Remove(people);
    //        _DB_Contex.SaveChanges();
    //    }

    //    public string ReturnFotoFilePeople(int id)
    //    {
    //        string Foto;
    //        People people = ReturnPeople(id);
    //        Foto = people.FotoName;
    //        return Foto;
    //    }

    //    public List<Project> ReturnDB_Projects()
    //    {
    //        return _DB_Contex.Projects.ToList();
    //    }

    //    public People ReturnPeople( int? id)
    //    {
    //       return _DB_Contex.Peoples.Find(id);
    //    }

    //    public void AddNewProject(Project project)
    //    {
    //        if (project != null)
    //        {
    //            _DB_Contex.Projects.Add(new Project { Name = project.Name, Price = project.Price });
    //            _DB_Contex.SaveChanges();
    //        }
    //    }

    //    public void PeopleInProject(int? PeopleID)
    //    {
    //        Project project = ReturnProject(ProjectID);
    //        People people = ReturnPeople(PeopleID);
    //        people.Projects.Add(project);
    //        _DB_Contex.SaveChanges();
    //    }

    //    public Project ReturnProject(int? id)
    //    {
    //        return _DB_Contex.Projects.Find(id);
    //    }

    //    public void EditeProject(Project project)
    //    {
    //        _DB_Contex.Entry(project).State = EntityState.Modified;
    //        _DB_Contex.SaveChanges();
    //    }

    //    public void DeleteProject(int id)
    //    {
    //        Project project = ReturnProject(id);
    //        _DB_Contex.Projects.Remove(project);
    //        _DB_Contex.SaveChanges();
    //    }

    //    public string[] Imagas()
    //    {
    //        var r =  _DB_Contex.Peoples.Where(t => t.FotoName != "");
    //        string[] w = new string[r.Count()];
    //        int counter = 0;
    //        foreach (var i in r)
    //        {
    //            w[counter] += i.FotoName;
    //            counter++;
    //        }
    //        return w;
    //    }
    //}
}