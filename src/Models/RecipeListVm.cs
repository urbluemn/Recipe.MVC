namespace Recipe.MVC.src.Models
{
    /// <summary>
    /// Recipe List viewmodel
    /// </summary>
    public class RecipeListVm
    {
        public int pageNumber { get; set; } = 0;
        public int pageSize { get; set; } = 1;
        public IList<RecipeLookupDto> Recipes { get; set; }
    }
}
