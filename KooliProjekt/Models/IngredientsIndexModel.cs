using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class IngredientsIndexModel
    {
        public IngredientSearch Search { get; set; } = new IngredientSearch();
        public PagedResult<Ingredient> Data { get; set; } = new PagedResult<Ingredient>();
    }
}
