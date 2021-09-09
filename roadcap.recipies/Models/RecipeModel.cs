using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace roadcap.recipes.Models
{
    public class RecipeModel
    {
        public int RecipeId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public string Instructions { get; set; }

        public IFormFile Image { get; set; }
    }
}
