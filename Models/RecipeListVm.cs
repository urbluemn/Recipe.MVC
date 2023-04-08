namespace Recipe.MVC.Models
{
    /// <summary>
    /// Recipe List viewmodel
    /// </summary>
    public class RecipeListVm
    {
        public IList<RecipeLookupDto> Recipes { get; set; }
    }
}
