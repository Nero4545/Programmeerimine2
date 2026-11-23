using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface IIngredientService
    {
        Task<PagedResult<Ingredient>> List(int page, int pageSize, IngredientSearch? search = null);
        Task<Ingredient?> Get(int id);
        Task Save(Ingredient item);
        Task Delete(int id);
    }
}
