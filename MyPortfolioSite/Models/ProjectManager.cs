using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;

namespace MyPortfolioSite.Models
{
    public class ProjectManager : IProjectRepository
    {
        private DB_Contex _db;

        public ProjectManager(DB_Contex db)
        {
            _db = db;
        }

        Task<int> IProjectRepository.CreateProject(Project project)
        {
            ProjectFromRepository dataBase = new ProjectFromRepository(_db);
            return dataBase.AddProject(project);
        }

        Task<bool> IProjectRepository.DeleteProject(int? Id)
        {
            ProjectFromRepository dataBase = new ProjectFromRepository(_db);
            return dataBase.DeleteProject(Id);
        }

        Task<bool> IProjectRepository.EditProject(Project project, int? Id)
        {
            ProjectFromRepository dataBase = new ProjectFromRepository(_db);
            return dataBase.EditProject(project, Id);
        }

        Task<Project> IProjectRepository.GetProject(int? Id)
        {
            ProjectFromRepository dataBase = new ProjectFromRepository(_db);
            return dataBase.GetProject(Id);
        }
    }
}