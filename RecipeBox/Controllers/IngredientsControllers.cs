using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System;

namespace RecipeBox.Controllers
{
    public class IngredientsController : Controller
    {
        [HttpGet("/ingredients")]
        public ActionResult Index()
        {
            List<Ingredient> allIngredients = Ingredient.GetAll();
            return View(allIngredients);
        }

        [HttpGet("/ingredients/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/ingredients")]
        public ActionResult Create(string ingredientName)
        {
            Ingredient newIngredient = new Ingredient(ingredientName);
            newIngredient.Save();
            List<Ingredient> allIngredients = Ingredient.GetAll();
            return View("Index", allIngredients);
        }

        [HttpGet("/ingredients/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Ingredient selectedIngredient = Ingredient.Find(id);
            List<Recipe> ingredientRecipes = selectedIngredient.GetRecipes();
            List<Recipe> allRecipes = Recipe.GetAll();
            model.Add("selectedIngredient", selectedIngredient);
            model.Add("ingredientRecipes", ingredientRecipes);
            model.Add("allRecipes", allRecipes);
            return View(model);
        }

        [HttpPost("/ingredients/{ingredientId}/recipes/new")]
        public ActionResult AddRecipe(int ingredientId, int recipeId)
        {
            Ingredient ingredient = Ingredient.Find(ingredientId);
            Recipe recipe = Recipe.Find(recipeId);
            ingredient.AddRecipe(recipe);
            return RedirectToAction("Show",  new { id = ingredientId });
        }
    }
}