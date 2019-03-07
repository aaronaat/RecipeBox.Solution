using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace RecipeBox.Models
{
    public class Category
    {
        private string _type;
        private int _id;

        public Category(string categoryType, int id = 0)
        {
            _type = categoryType;
            _id = id;
        }

        public string GetType()
        {
            return _type;
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
            cmd.CommandText = @"DELETE FROM categories;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Category> GetAll()
        {
            List<Category> allCategories = new List<Category> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int CategoryId = rdr.GetInt32(0);
                string CategoryType = rdr.GetString(1);
                Category newCategory = new Category(CategoryType, CategoryId);
                allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCategories;
        }

        public static Category Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int CategoryId = 0;
            string CategoryType = "";
            while (rdr.Read())
            {
                CategoryId = rdr.GetInt32(0);
                CategoryType = rdr.GetString(1);
            }
            Category newCategory = new Category(CategoryType, CategoryId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCategory;
        }

        public List<Recipe> GetRecipes(string sortBy = "")
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            if  (sortBy == "")
            {
                cmd.CommandText = @"SELECT recipes.* FROM categories 
                    JOIN recipes_categories ON (categories.id = recipes_categories.category_id) 
                    JOIN recipes ON (recipes_categories.recipe_id = recipes.id)
                    WHERE categories.id = @CategoryId;";
            } else {
                cmd.CommandText = @"SELECT recipes.* FROM categories 
                    JOIN recipes_categories ON (categories.id = recipes_categories.category_id) 
                    JOIN recipes ON (recipes_categories.recipe_id = recipes.id)
                    WHERE categories.id = @CategoryId ORDER BY recipes." + sortBy + ";";
            }
            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@CategoryId";
            categoryId.Value = _id;
            cmd.Parameters.Add(categoryId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Recipe> recipes = new List<Recipe> {};
            while(rdr.Read())
            {
                int thisRecipeId = rdr.GetInt32(0);
                string recipeName = rdr.GetString(1);
                string recipeInstructions = rdr.GetString(2);
                int recipeRating = rdr.GetInt32(3);
                Recipe foundRecipe = new Recipe(recipeName, recipeInstructions, recipeRating, thisRecipeId);
                recipes.Add(foundRecipe);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return recipes;
        }

        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category)otherCategory;
                bool idEquality = this.GetId().Equals(newCategory.GetId());
                bool nameEquality = this.GetType().Equals(newCategory.GetType());
                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories (type) VALUES (@type);";
            MySqlParameter type = new MySqlParameter();
            type.ParameterName = "@type";
            type.Value = this._type;
            cmd.Parameters.Add(type);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
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
            MySqlCommand cmd = new MySqlCommand("DELETE FROM categories Where id = @CategoryId; DELETE FROM recipes_categories WHERE category_id = @CategoryId;", conn);
            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@CategoryId";
            categoryId.Value = this.GetId();
            cmd.Parameters.Add(categoryId);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public void AddRecipe(Recipe newRecipe)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipes_categories (category_id, recipe_id) VALUES (@CategoryId, @RecipeId);";
            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CategoryId";
            category_id.Value = _id;
            cmd.Parameters.Add(category_id);
            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@RecipeId";
            recipe_id.Value = newRecipe.GetId();
            cmd.Parameters.Add(recipe_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}