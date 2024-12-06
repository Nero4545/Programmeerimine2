using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IManufacturingLogService
    {
        Task<IEnumerable<ManufacturingLog>> GetAll();
        Task<ManufacturingLog> GetById(int id);
        Task Save(ManufacturingLog ManufacturingLog);
        Task Delete(int id);

        Task<List<ManufacturingLog>> ListAsync(ManufacturingLogSearchParameters searchParameters);
    }
}