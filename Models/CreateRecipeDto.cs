using System.ComponentModel.DataAnnotations;

namespace Recipe.MVC.Models
{
    /// <summary>
    /// Create recipe viewmodel
    /// </summary>
    public class CreateRecipeDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Details { get; set; }
    }
}
