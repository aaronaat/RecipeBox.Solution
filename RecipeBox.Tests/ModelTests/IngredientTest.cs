using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System.Collections.Generic;
using System;

namespace RecipeBox.Tests
{
    [TestClass]
    public class IngredientTest : IDisposable
    {

        public IngredientTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
        }

        public void Dispose()
        {
            Ingredient.ClearAll();
            //Ingredient.ClearAll();
        }



[TestMethod]
        public void GetRecipes_ReturnsEmptyRecipeList_RecipeList()
        {
            //Arrange
            string type = "Work";
            Ingredient newRecipe = new Ingredient(type);
            List<Ingredient> newList = new List<Ingredient> { };

            //Act
            List<Recipe> result = newRecipe.GetRecipes();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }
    }
}