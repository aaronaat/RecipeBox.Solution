using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
    public class CategoriesController : Controller
    {
        [HttpGet("/categories")]
        public ActionResult Index()
        {
            List<Category> allCategories = Category.GetAll();
            return View(allCategories);
        }

        [HttpGet("/categories/new")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost("/categories")]
        public ActionResult Create(string type)
        {
            Category newCategory = new Category(type);
            newCategory.Save();
            List<Category> allCategories = Category.GetAll();
            return View("Index", allCategories);
        }

        [HttpGet("/categories/{id}")]
        public ActionResult Show(int id)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Category selectedCategory = Category.Find(id);
            List<Recipe> categoryRecipes = selectedCategory.GetRecipes();
            List<Recipe> allRecipes = Recipe.GetAll();
            model.Add("category", selectedCategory);
            model.Add("categoryRecipes", categoryRecipes);
            model.Add("allRecipes", allRecipes);
            return View(model);
        }

        [HttpPost("/categories/{categoryId}/recipes/new")]
        public ActionResult AddRecipe(int categoryId, int recipeId)
        {
            Category category = Category.Find(categoryId);
            Recipe recipe = Recipe.Find(recipeId);
            category.AddRecipe(recipe);
            return RedirectToAction("Show", new { id = categoryId });
        }
    }
}