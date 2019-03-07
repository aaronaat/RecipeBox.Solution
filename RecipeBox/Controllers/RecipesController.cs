using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
    public class RecipesController : Controller
    {
        [HttpGet("/recipes")]
        public ActionResult Index()
        {
            List<Recipe> allRecipes = Recipe.GetAll();
            return View(allRecipes);
        }

        [HttpGet("/recipes/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/recipes")]
        public ActionResult Create(string recipeName, string recipeInstructions, int recipeRating)
        {
            Recipe newRecipe = new Recipe(recipeName, recipeInstructions, recipeRating);
            newRecipe.Save();
            List<Recipe> allRecipes = Recipe.GetAll();
            return View("Index", allRecipes);
        }

        [HttpGet("/recipes/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Recipe selectedRecipe = Recipe.Find(id);
            List<Ingredient> recipeIngredients = selectedRecipe.GetIngredients();
            List<Ingredient> allIngredients = Ingredient.GetAll();
            model.Add("recipe", selectedRecipe);
            model.Add("recipeIngredients", recipeIngredients);
            model.Add("allIngredients", allIngredients);
            return View(model);
        }

        [HttpPost("/recipes/{recipeId}/ingredients/new")]
        public ActionResult AddIngredient(int recipeId, int ingredientId)
        {
            Recipe recipe = Recipe.Find(recipeId);
            Ingredient ingredient = Ingredient.Find(ingredientId);
            recipe.AddIngredient(ingredient);
            return RedirectToAction("Show", new { id = recipeId });
        }
    }
}