using MyPortfolioSite.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioSite.Models
{
    public interface IContext
    {   //People
        void AddNewPeople(People people);
        void EditePeople(People people);
        void DeletePeople(int id);
        List<People> ReturneDB_People();        
        People ReturnPeople(int? id);
        void PeopleInProject(int? PeopleID);
        
        //Project
        void AddNewProject(Project project);
        void EditeProject(Project project);
        void DeleteProject(int id);
        Project ReturnProject(int? id);
        List<Project> ReturnDB_Projects();
    }
}
