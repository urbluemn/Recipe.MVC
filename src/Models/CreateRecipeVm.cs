namespace Recipe.MVC.src.Models
{
    public class CreateRecipeVm
    {
        public CreateRecipeDto Recipe { get; set;}
        public IFormFile RecipeImage { get; set; }
    }
}