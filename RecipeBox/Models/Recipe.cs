using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace RecipeBox.Models
{
    public class Recipe
    {
        private string _recipeName;
        private string _instructions;
        private int _rating;
        private int _id;
        
        public Recipe(string recipeName, string instructions, int rating, int id = 0)
        {
            _recipeName = recipeName;
            _instructions = instructions;
            _rating = rating;
            _id = id;
        }

        public string GetRecipeName()
        {
            return _recipeName;
        }

        public string GetInstructions()
        {
            return _instructions;
        }

        public int GetRating()
        {
            return _rating;
        }

        public int GetId()
        {
            return _id;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM recipes;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Recipe> GetAll(string sortBy = "")
        {
            List<Recipe> allRecipes = new List<Recipe> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;

            if (sortBy == "")
            {
                cmd.CommandText = @"SELECT * FROM recipes;";
            } else {
                cmd.CommandText = @"SELECT * FROM recipes ORDER BY " + sortBy + ";";
            }
            
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int recipeId = rdr.GetInt32(0);
                string recipeName = rdr.GetString(1);
                string recipeInstructions = rdr.GetString(2);
                int recipeRating = rdr.GetInt32(3);
                Recipe newRecipe = new Recipe(recipeName, recipeInstructions, recipeRating, recipeId);
                allRecipes.Add(newRecipe);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allRecipes;
        }

        public static Recipe Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM recipes WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int recipeId = 0;
            string recipeName = "";
            string recipeInstructions = "";
            int recipeRating = 0;
            while (rdr.Read())
            {
                recipeId = rdr.GetInt32(0);
                recipeName = rdr.GetString(1);
                recipeInstructions = rdr.GetString(2);
                recipeRating = rdr.GetInt32(3);
            }
            Recipe newRecipe = new Recipe(recipeName, recipeInstructions, recipeRating, recipeId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newRecipe;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipes (name, instructions, rating) VALUES (@name, @instructions, @rating);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._recipeName;
            cmd.Parameters.Add(name);

            MySqlParameter instructions = new MySqlParameter();
            instructions.ParameterName = "@instructions";
            instructions.Value = this._instructions;
            cmd.Parameters.Add(instructions);

            MySqlParameter rating = new MySqlParameter();
            rating.ParameterName = "@rating";
            rating.Value = this._rating;
            cmd.Parameters.Add(rating);

            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Ingredient> GetIngredients()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"SELECT ingredients.* FROM recipes 
                    JOIN recipes_ingredients ON (recipes.id = recipes_ingredients.recipe_id) 
                    JOIN ingredients ON (recipes_ingredients.ingredient_id = ingredients.id)
                    WHERE recipes.id = @RecipeId;";
            MySqlParameter recipeId = new MySqlParameter();
            recipeId.ParameterName = "@RecipeId";
            recipeId.Value = _id;
            cmd.Parameters.Add(recipeId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Ingredient> ingredients = new List<Ingredient> {};
            while(rdr.Read())
            {
                int thisIngredientId = rdr.GetInt32(0);
                string ingredientName = rdr.GetString(1);
                Ingredient foundIngredient = new Ingredient(ingredientName, thisIngredientId);
                ingredients.Add(foundIngredient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return ingredients;
        }

        public void AddIngredient(Ingredient newIngredient)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipes_ingredients (recipe_id, ingredient_id) VALUES (@RecipeId, @IngredientId);";
            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@RecipeId";
            recipe_id.Value = _id;
            cmd.Parameters.Add(recipe_id);
            MySqlParameter Ingredient_id = new MySqlParameter();
            Ingredient_id.ParameterName = "@IngredientId";
            Ingredient_id.Value = newIngredient.GetId();
            cmd.Parameters.Add(Ingredient_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherRecipe)
        {
            if (!(otherRecipe is Recipe))
            {
                return false;
            }
            else
            {
                Recipe newRecipe = (Recipe)otherRecipe;
                bool idEquality = this.GetId().Equals(newRecipe.GetId());
                bool nameEquality = this.GetRecipeName().Equals(newRecipe.GetRecipeName());
                return (idEquality && nameEquality);
            }
        }

    }
}