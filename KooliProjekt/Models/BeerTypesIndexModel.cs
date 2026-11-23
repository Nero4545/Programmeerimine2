using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class BeerTypesIndexModel
    {
        public BeerTypeSearch Search { get; set; } = new BeerTypeSearch();
        public PagedResult<BeerType> Data { get; set; } = new PagedResult<BeerType>();
    }
}
