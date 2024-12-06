using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IProductComponentService
    {
        Task<IEnumerable<ProductComponent>> GetAll();
        Task<ProductComponent> GetById(int id);
        Task Save(ProductComponent ProductComponents);
        Task Delete(int id);

        Task<List<ProductComponent>> ListAsync(ProductComponentSearchParameters searchParameters);
    }
}
