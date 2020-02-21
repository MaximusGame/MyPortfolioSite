using System.Threading.Tasks;
using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;

namespace MyPortfolioSite.Models
{
    public class PeopleManager : IPeopleRepository
    {
        private DB_Contex _db;

        public PeopleManager(DB_Contex db)
        {
            _db = db;
        }

        Task<int> IPeopleRepository.CreatePeople(People people)
        {
            PeopleFromRepository dataBase = new PeopleFromRepository(_db);
            return dataBase.AddPeopleInDB(people);
        }

        Task<bool> IPeopleRepository.DeletePeople(int? Id)
        {
            PeopleFromRepository dataBase = new PeopleFromRepository(_db);
            return dataBase.DeletePeople(Id);
        }

        Task<bool> IPeopleRepository.EditPeople(People people, int? Id)
        {
            PeopleFromRepository dataBase = new PeopleFromRepository(_db);
            return dataBase.EditPeople(people, Id);
        }

        Task<People> IPeopleRepository.GetPeople(int? Id)
        {
            PeopleFromRepository dataBase = new PeopleFromRepository(_db);
            return dataBase.GetPeople(Id);
        }
    }
}