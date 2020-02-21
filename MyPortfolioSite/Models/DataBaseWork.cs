using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MyPortfolioSite.Models
{
    public interface IPeopleFromLocalDB
    {
        Task<People> GetPeopleFromDB(int? Id);
        Task<int> AddPeopleInDB(People people);
        Task<bool> DeletePeopleFromDB(int? Id);
        Task<bool> EditPeopleInDB(People people, int? Id);
    }

    public interface IProjectFromLocalDB
    {
        Task<Project> GetProjectFromDB(int? Id);
        Task<int> AddProjectInDB(Project project);
        Task<bool> DeleteProjectFromDB(int? Id);
        Task<bool> EditProjectInDB(Project project, int? Id);
    }


    class PersonLocalDB_Qwery : IPeopleFromLocalDB
    {
        private readonly DB_Contex _db;

        public PersonLocalDB_Qwery(DB_Contex db)
        {
            _db = db;
        }

        async Task<int> IPeopleFromLocalDB.AddPeopleInDB(People people)
        {
            if (people.FotoName == null)
            {
                people.FotoName = "avatar.png";
            }
            _db.Peoples.Add(people);
            await _db.SaveChangesAsync();
            var a = _db.Peoples.Where(p => p.Name == people.Name);
            int r = a.FirstOrDefault().Id;
            return r;
        }

        async Task<bool> IPeopleFromLocalDB.DeletePeopleFromDB(int? Id)
        {
            People people = await _db.Peoples.FindAsync(Id);
            if (people != null)
            {
                if ("avatar.png" != people.FotoName)
                {
                    System.IO.File.Delete(HttpContext.Current.Server.MapPath("~/Fotos/" + people.FotoName));
                }
                _db.Peoples.Remove(people);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        async Task<bool> IPeopleFromLocalDB.EditPeopleInDB(People people, int? Id)
        {
            People Edit_people = await _db.Peoples.FindAsync(Id);

            if (Edit_people != null)
            {
                Edit_people.Name = people.Name;
                Edit_people.Phone = people.Phone;

                if (people.FotoName != null)
                {
                    Edit_people.FotoName = people.FotoName;
                }

               _db.Entry(Edit_people).State = EntityState.Modified;
               _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }

        async Task<People> IPeopleFromLocalDB.GetPeopleFromDB(int? Id)
        {
            People people = await _db.Peoples.FindAsync(Id);
            return people;
        }
    }

    public class PeopleFromRepository
    {
        private IPeopleFromLocalDB _local_repo;

        public PeopleFromRepository(DB_Contex db)
        {
            _local_repo = new PersonLocalDB_Qwery(db);
        }

        public Task<int> AddPeopleInDB(People people)
        {
            return _local_repo.AddPeopleInDB(people);
        }

        public Task<bool> EditPeople(People people, int? Id)
        {
           return _local_repo.EditPeopleInDB(people, Id);
        }

        public Task<People> GetPeople(int? id)
        {
            return _local_repo.GetPeopleFromDB(id);
        }

        public Task<bool> DeletePeople(int? Id)
        {
            return _local_repo.DeletePeopleFromDB(Id);
        }

    }

    class ProjectLocalDB_Qwery : IProjectFromLocalDB
    {
        private readonly DB_Contex _db;

        public ProjectLocalDB_Qwery(DB_Contex db)
        {
            _db = db;
        }

        async Task<int> IProjectFromLocalDB.AddProjectInDB(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
            var a = _db.Projects.Where(p => p.Name == project.Name);
            int r = a.FirstOrDefault().Id;
            return r;
        }

        async Task<bool> IProjectFromLocalDB.DeleteProjectFromDB(int? Id)
        {
            Project project = await _db.Projects.FindAsync(Id);
            if (project != null)
            {
                _db.Projects.Remove(project);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        async Task<bool> IProjectFromLocalDB.EditProjectInDB(Project project, int? Id)
        {
            Project Edit_project = await _db.Projects.FindAsync(Id);

            if (Edit_project != null)
            {
                Edit_project.Name = project.Name;
                Edit_project.Price = project.Price;
                _db.Entry(Edit_project).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        async Task<Project> IProjectFromLocalDB.GetProjectFromDB(int? Id)
        {
            Project project = await _db.Projects.FindAsync(Id);
            return project;
        }
    }

    public class ProjectFromRepository
    {
        IProjectFromLocalDB _local_repo;

        public ProjectFromRepository(DB_Contex db)
        {
            _local_repo = new ProjectLocalDB_Qwery(db);
        }

        public Task<int> AddProject(Project project)
        {
            return _local_repo.AddProjectInDB(project);
        }

        public Task<bool> EditProject(Project project, int? Id)
        {
            return _local_repo.EditProjectInDB(project, Id);
        }

        public Task<Project> GetProject(int? id)
        {
            return _local_repo.GetProjectFromDB(id);
        }

        public Task<bool> DeleteProject(int? Id)
        {
            return _local_repo.DeleteProjectFromDB(Id);
        }

    }
}