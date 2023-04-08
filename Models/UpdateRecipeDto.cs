namespace Recipe.MVC.Models
{
    /// <summary>
    /// Update recipe viewmodel
    /// </summary>
    public class UpdateRecipeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
    }
}
