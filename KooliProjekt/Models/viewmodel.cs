using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models
{
    public class OilTypesIndexModel
    {
        public OilTypeSearchParameters SearchParameters { get; set; }
        public List<OilType> Items { get; set; }
    }
}
