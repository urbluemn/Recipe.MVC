namespace Recipe.MVC.src.Models
{
    /// <summary>
    /// Viewmodel to show in a List
    /// </summary>
    public class RecipeLookupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
