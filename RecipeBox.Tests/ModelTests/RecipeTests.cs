using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System.Collections.Generic;
using System;

namespace RecipeBox.Tests
{
    [TestClass]
    public class RecipeTest : IDisposable
    {

        public RecipeTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
        }

        public void Dispose()
        {
            Recipe.ClearAll();
            //Ingredient.ClearAll();
        }

        [TestMethod]
        public void RecipeConstructor_CreatesInstanceOfRecipe_Recipe()
        {
            Recipe newRecipe = new Recipe("test Recipe", "yadda yadda", 9, 1);
            Assert.AreEqual(typeof(Recipe), newRecipe.GetType());
        }

        [TestMethod]
        public void GetName_ReturnsName_String()
        {
            //Arrange
            string name = "Test Recipe";
            string instructions = "yadda";
            int rating = 9;
            Recipe newRecipe = new Recipe(name, instructions, rating);

            //Act
            string result = newRecipe.GetRecipeName();

            //Assert
            Assert.AreEqual(name, result);
        }

        [TestMethod]
        public void GetId_ReturnsRecipeId_Int()
        {
            //Arrange
            string name = "Test Recipe";
            string instructions = "yadda";
            int rating = 9;
            Recipe newRecipe = new Recipe(name, instructions, rating);

            //Act
            int result = newRecipe.GetId();

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetAll_ReturnsAllRecipeObjects_RecipeList()
        {
            //Arrange
            string name01 = "Work";
            string instructions1 = "yadda";
            int rating1 = 9;
            string name02 = "School";
            string instructions2 = "yadda";
            int rating2 = 9;
            Recipe newRecipe1 = new Recipe(name01, instructions1, rating1);
            newRecipe1.Save();
            Recipe newRecipe2 = new Recipe(name02, instructions2, rating2);
            newRecipe2.Save();
            List<Recipe> newList = new List<Recipe> { newRecipe1, newRecipe2 };

            //Act
            List<Recipe> result = Recipe.GetAll();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_ReturnsRecipeInDatabase_Recipe()
        {
            //Arrange
            Recipe testRecipe = new Recipe("Household chores", "yadda yadda", 9);
            testRecipe.Save();

            //Act
            Recipe foundRecipe = Recipe.Find(testRecipe.GetId());

            //Assert
            Assert.AreEqual(testRecipe, foundRecipe);
        }

        [TestMethod]
        public void GetIngredients_ReturnsEmptyIngredientList_IngredientList()
        {
            //Arrange
            string name = "Work";
            string instructions = "yadda";
            int rating = 9;
            Recipe newRecipe = new Recipe(name, instructions, rating);
            List<Ingredient> newList = new List<Ingredient> { };

            //Act
            List<Ingredient> result = newRecipe.GetIngredients();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Save_SavesRecipeToDatabase_RecipeList()
        {
            //Arrange
            Recipe testRecipe = new Recipe("Household chores", "yadda yadda", 3);
            testRecipe.Save();

            //Act
            List<Recipe> result = Recipe.GetAll();
            List<Recipe> testList = new List<Recipe> { testRecipe };

            //Assert
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void Save_DatabaseAssignsIdToRecipe_Id()
        {
            //Arrange
            Recipe testRecipe = new Recipe("Household chores", "yadda", 3);
            testRecipe.Save();

            //Act
            Recipe savedRecipe = Recipe.GetAll()[0];

            int result = savedRecipe.GetId();
            int testId = testRecipe.GetId();

            //Assert
            Assert.AreEqual(testId, result);
        }

        [TestMethod]
        public void Test_AddIngredient_AddsIngredientToRecipe()
        {
            Recipe testRecipe = new Recipe("Banana Pie", "yadda", 9);
            testRecipe.Save();
            Ingredient testIngredient = new Ingredient("Banana");
            testIngredient.Save();
            Ingredient testIngredient2 = new Ingredient("Pie");
            testIngredient2.Save();
            testRecipe.AddIngredient(testIngredient);
            testRecipe.AddIngredient(testIngredient2);
            List<Ingredient> result = testRecipe.GetIngredients();
            List<Ingredient> testList = new List<Ingredient>{testIngredient, testIngredient2};
            CollectionAssert.AreEqual(testList, result);
        }

        [TestMethod]
        public void GetIngredients_ReturnAllRecipeIngredient_IngredientList()
        {
            Recipe testRecipe = new Recipe("Banana Pie", "yadda", 9);
            testRecipe.Save();
            Ingredient testIngredient1 = new Ingredient("Banana");
            testIngredient1.Save();
            Ingredient testIngredient2 = new Ingredient("Pie");
            testIngredient2.Save();

            testRecipe.AddIngredient(testIngredient1);
            List<Ingredient> savedIngredients = testRecipe.GetIngredients();
            List<Ingredient> testList = new List<Ingredient> {testIngredient1};
            CollectionAssert.AreEqual(testList, savedIngredients);
        }
    }
}