using Microsoft.AspNetCore.Mvc;
using roadcap.recipes.entities.Contexts;
using roadcap.recipes.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using roadcap.recipes.business;

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
        public async Task<IActionResult> Find(string searchTerm)
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
                results = await this._context.Recipes.Where(r =>
                    r.Title.Contains(searchTerm) ||
                    r.Description.Contains(searchTerm) ||
                    r.Ingredients.Any(i => i.IngredientName.Contains(searchTerm))
                ).ToListAsync();
            }

            ViewData["searchTerm"] = searchTerm;
            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.RecipeId == id);

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
                // Apply business rules
                var businessRule = new ABusinessRule();
                recipe = businessRule.DoSomethingBusinessy(recipe);

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

        [HttpGet]
        public IActionResult Add(int id)
        {
            var recipe = new Recipe();

            ViewData["action"] = "Add";

            return View("Add", recipe);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Recipe recipe)
        {
            try
            {
                // Apply business rules
                var businessRule = new ABusinessRule();
                recipe = businessRule.DoSomethingBusinessy(recipe);

                _context.Add(recipe);
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
