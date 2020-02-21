using MyPortfolioSite.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPortfolioSite.Models
{
    interface IProjectRepository
    {
        Task<Project> GetProject(int? Id);
        Task<int> CreateProject(Project project);
        Task<bool> DeleteProject(int? Id);
        Task<bool> EditProject(Project project, int? Id);
    }
}
