using Microsoft.AspNetCore.Mvc;
using roadcap.recipes.entities.Contexts;
using roadcap.recipes.entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using roadcap.recipes.business;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        public async Task<IActionResult> Edit(Recipe recipe, IFormFile Image)
        {
            try
            {
                // Apply business rules
                var businessRule = new ABusinessRule();
                recipe = businessRule.DoSomethingBusinessy(recipe);

                if (Image != null && Image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await Image.CopyToAsync(ms);
                        recipe.Image = ms.ToArray();
                    }
                }
                else
                {
                    // make sure we don't write over the old image
                    var tempRecipe = _context.Recipes.AsNoTracking().FirstOrDefault(r => r.RecipeId == recipe.RecipeId);
                    recipe.Image = tempRecipe.Image;
                }

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

        [HttpGet]
        public async Task<IActionResult> GetImage(int id)
        {
            var recipe = await _context.Recipes.Include(r => r.Ingredients).FirstOrDefaultAsync(r => r.RecipeId == id);
            if (recipe.Image != null && recipe.Image.Length > 0)
            {
                var ms = new MemoryStream(recipe.Image);
                return new FileStreamResult(ms, "image/png");
            }

            return new EmptyResult();
        }
    }
}
