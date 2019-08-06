using EatIn.Models.API_models;
using EatIn.Models.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatIn.Models.ViewModels
{
    public class API_RecipeVM
    {
        public int id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public List<API_ingredient> missedIngredients { get; set; }
        public List<API_ingredient> usedIngredients { get; set; }
        public API_recipe_details recipeDetails { get; set; }

        public API_RecipeVM(API_recipe apiRecipe, API api)
        {
            id = apiRecipe.id;
            title = apiRecipe.title;
            image = apiRecipe.image;
            missedIngredients = apiRecipe.missedIngredients;
            usedIngredients = apiRecipe.usedIngredients;
            recipeDetails = api.GetDetails(id, false);
        }

    }
}