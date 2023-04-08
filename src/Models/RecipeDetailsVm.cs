namespace Recipe.MVC.src.Models
{
    /// <summary>
    /// Recipe details viewmodel
    /// </summary>
    public class RecipeDetailsVm
    {
        public string Username { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
