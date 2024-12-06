using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IOilTypeService
    {
        Task<IEnumerable<OilType>> GetAll();
        Task<OilType> GetById(int id);
        Task Save(OilType oilType);
        Task Delete(int id);

        Task<List<OilType>> ListAsync(OilTypeSearchParameters searchParameters);
    }
}