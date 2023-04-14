using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe.MVC.src.Models
{
    public class RecipeImage
    {
        public Guid RecipeId { get; set;}
        public Guid Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImagePath { get; set; }
    }
}