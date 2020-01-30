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
            CreatePeaple_Query createPeaple_Query = new CreatePeaple_Query(_db);
            return createPeaple_Query.ExecuteAsync(people);
        }

        Task<bool> IPeopleRepository.DeletePeople(int? Id)
        {
            DeletePeaple_Query deletePeaple_Query = new DeletePeaple_Query(_db);
            return deletePeaple_Query.ExecuteAsync(Id);
        }

        Task<People> IPeopleRepository.GetPeople(int? Id)
        {
            GetPeopleById_Query GetPeopleById_Query = new GetPeopleById_Query(_db);
            return GetPeopleById_Query.ExecuteAsync(Id);
        }
    }
}