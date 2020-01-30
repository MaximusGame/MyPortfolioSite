using MyPortfolioSite.Context;
using MyPortfolioSite.DataModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolioSite.Models
{
    public class GetPeopleById_Query
    {
        private readonly DB_Contex _db;

        public GetPeopleById_Query(DB_Contex db)
        {
            _db = db;
        }

        public async Task<People> ExecuteAsync(int? id)
        {
            People people = await _db.Peoples.FindAsync(id);
            return people;
        }

    }

    public class CreatePeaple_Query
    {
        private readonly DB_Contex _db;

        public CreatePeaple_Query(DB_Contex db)
        {
            _db = db;
        }

        public async Task<int> ExecuteAsync(People people)
        {
            people.FotoName = "avatar.png";
            _db.Peoples.Add(people);
            await _db.SaveChangesAsync();
            var a = _db.Peoples.Where(p => p.Name == people.Name);
            int r = a.FirstOrDefault().Id;
            return r;
        }

    }

    public class DeletePeaple_Query
    {
        private readonly DB_Contex _db;

        public DeletePeaple_Query(DB_Contex db)
        {
            _db = db;
        }

        public async Task<bool> ExecuteAsync(int? id)
        {
            People people = await _db.Peoples.FindAsync(id);
            if (people != null)
            {
                _db.Peoples.Remove(people);
                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}