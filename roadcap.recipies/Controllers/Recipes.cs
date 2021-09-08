using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace roadcap.recipes.Controllers
{
    public class Recipes : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
