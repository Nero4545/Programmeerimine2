using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Search
{
    public class IngredientSearch
    {
        public string? Keyword { get; set; }
        public int? BatchId { get; set; }
    }
}
