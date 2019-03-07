using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace RecipeBox.Models
{
    public class Ingredient
    {
        private string _ingredientName;
        private int _id;

        public Ingredient(string ingredientName, int id = 0)
        {
            _ingredientName = ingredientName;
            _id = id;
        }

        public string GetName()
        {
            return _ingredientName;
        }

        public void SetName(string newIngredientName)
        {
            _ingredientName = newIngredientName;
        }


        public int GetId()
        {
            return _id;
        }

        public static List<Ingredient> GetAll()
        {
            List<Ingredient> allIngredients = new List<Ingredient> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM ingredients;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int ingredientId = rdr.GetInt32(0);
                string ingredientName = rdr.GetString(1);
                Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
                allIngredients.Add(newIngredient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allIngredients;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM ingredients;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM ingredients WHERE id = @ingredientId; DELETE FROM recipes_ingredients WHERE ingredient_id = @ingredientId;";
            MySqlParameter ingredientIdParameter = new MySqlParameter();
            ingredientIdParameter.ParameterName = "@ingredientId";
            ingredientIdParameter.Value = this.GetId();
            cmd.Parameters.Add(ingredientIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Ingredient Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM ingredients WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int ingredientId = 0;
            string ingredientName = "";
            while (rdr.Read())
            {
                ingredientId = rdr.GetInt32(0);
                ingredientName = rdr.GetString(1);
            }
            Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newIngredient;
        }

        public override bool Equals(System.Object otherIngredient)
        {
            if (!(otherIngredient is Ingredient))
            {
                return false;
            }
            else
            {
                Ingredient newIngredient = (Ingredient)otherIngredient;
                bool idEquality = (this.GetId() == newIngredient.GetId());
                bool nameEquality = (this.GetName() == newIngredient.GetName());
                return (idEquality && nameEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO ingredients (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._ingredientName;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(string newIngredientName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE ingredients SET name = @newIngredientName WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newIngredientName";
            name.Value = newIngredientName;
            cmd.Parameters.Add(name);
            
            cmd.ExecuteNonQuery();
            _ingredientName = newIngredientName;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            } 
        }

        public List<Recipe> GetRecipes()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT recipes.* FROM ingredients
                JOIN recipes_ingredients ON (ingredients.id = recipes_ingredients.ingredient_id)
                JOIN recipes ON (recipes_ingredients.recipe_id = recipes.id)
                WHERE ingredients.id = @IngredientId;";
            MySqlParameter ingredientIdParameter = new MySqlParameter();
            ingredientIdParameter.ParameterName = "@IngredientId";
            ingredientIdParameter.Value = _id;
            cmd.Parameters.Add(ingredientIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Recipe> recipes = new List<Recipe> {};
            while(rdr.Read())
            {
                int thisRecipeId = rdr.GetInt32(0);
                string thisRecipeName = rdr.GetString(1);
                string thisRecipeInstructions = rdr.GetString(2);
                int thisRecipeRating = rdr.GetInt32(3);

                Recipe foundRecipe = new Recipe(thisRecipeName, thisRecipeInstructions, thisRecipeRating, thisRecipeId);
                recipes.Add(foundRecipe);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return recipes;
        }

        public void AddRecipe(Recipe newRecipe)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipes_ingredients (recipe_id, ingredient_id) VALUES (@RecipeId, @IngredientId);";
            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@RecipeId";
            recipe_id.Value = newRecipe.GetId();
            cmd.Parameters.Add(recipe_id);
            MySqlParameter ingredient_id = new MySqlParameter();
            ingredient_id.ParameterName = "@IngredientId";
            ingredient_id.Value = _id;
            cmd.Parameters.Add(ingredient_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}