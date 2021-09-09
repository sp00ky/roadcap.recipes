using Microsoft.AspNetCore.Mvc;
using roadcap.recipes.entities.Contexts;
using roadcap.recipes.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace roadcap.recipes.Controllers
{
    public class RecipesController : Controller
    {
        RoadcapRecipesContext _context;

        public RecipesController(RoadcapRecipesContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Find");
        }

        [HttpGet]
        public IActionResult Find(string searchTerm)
        {
            if (TempData["alert"] != null)
            {
                ViewData["alert"] = TempData["alert"];
            }

            IEnumerable<Recipe> results = null;
            if (searchTerm is null || searchTerm == string.Empty)
            {
                results = this._context.Recipes;
            }
            else
            {
                results = this._context.Recipes.Where(r =>
                    r.Title.Contains(searchTerm) ||
                    r.Description.Contains(searchTerm) ||
                    r.Ingredients.Any(i => i.IngredientName.Contains(searchTerm))
                );
            }

            ViewData["searchTerm"] = searchTerm;
            return View(results);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var recipe = _context.Recipes.Include(r => r.Ingredients).FirstOrDefault(r => r.RecipeId == id);

            if (TempData["alert"] != null)
            {
                ViewData["alert"] = TempData["alert"];
            }

            return View("Edit", recipe);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Recipe recipe)
        {
            try
            {
                _context.Update(recipe);
                await _context.SaveChangesAsync();
                TempData["alert"] = "Success";
            }
            catch (Exception ex)
            {
                TempData["alert"] = "Failed";

                // log exception
            }

            return RedirectToAction("Edit", new { id = recipe.RecipeId });
        }
    }
}
