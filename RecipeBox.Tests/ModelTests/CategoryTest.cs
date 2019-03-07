using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System.Collections.Generic;
using System;

namespace RecipeBox.Tests
{
    [TestClass]
    public class CategoryTest : IDisposable
    {

        public CategoryTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
        }

        public void Dispose()
        {
            Category.ClearAll();
            //Ingredient.ClearAll();
        }



[TestMethod]
        public void GetRecipes_ReturnsEmptyRecipeList_RecipeList()
        {
            //Arrange
            string type = "Work";
            Category newRecipe = new Category(type);
            List<Category> newList = new List<Category> { };

            //Act
            List<Recipe> result = newRecipe.GetRecipes();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }
    }
}