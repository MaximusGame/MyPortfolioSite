using MyPortfolioSite.DataModel;
using System.Threading.Tasks;

namespace MyPortfolioSite.Models
{
    interface IPeopleRepository
    {
       Task<People> GetPeople(int? Id);
       Task<int> CreatePeople(People people);
       Task<bool> DeletePeople(int? Id);
       Task<bool> EditPeople(People people, int? Id);
    }
}
