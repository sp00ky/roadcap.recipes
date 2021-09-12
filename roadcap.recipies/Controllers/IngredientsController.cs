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
    public class IngredientsController : Controller
    {
        RoadcapRecipesContext _context;

        public IngredientsController(RoadcapRecipesContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ingredient = _context.Ingredients.FirstOrDefault(r => r.IngredientId == id);

            if (TempData["alert"] != null)
            {
                ViewData["alert"] = TempData["alert"];
            }

            return View("Edit", ingredient);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                ViewData["alert"] = "Failed";
                ViewData["action"] = "Edit";
                return View(ingredient);
            }

            try
            {
                _context.Update(ingredient);
                await _context.SaveChangesAsync();
                TempData["alert"] = "Success";
            }
            catch (Exception ex)
            {
                TempData["alert"] = "Failed";

                // log exception
            }

            return RedirectToAction("Edit", "Recipes", new { id = ingredient.RecipeId });
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            var ingredient = new Ingredient { RecipeId = id };

            ViewData["action"] = "Add";

            return View(ingredient);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Ingredient ingredient)
        {
            if (!ModelState.IsValid)
            {
                ViewData["alert"] = "Failed";
                ViewData["action"] = "Add";
                return View(ingredient);
            }

            try
            {
                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                TempData["alert"] = "Success";
            }
            catch (Exception ex)
            {
                TempData["alert"] = "Failed";

                // log exception
            }

            return RedirectToAction("Edit", "Recipes", new { id = ingredient.RecipeId });
        }
    }
}
