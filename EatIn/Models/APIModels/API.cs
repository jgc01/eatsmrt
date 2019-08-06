using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using System.Net.Http;
using EatIn.Models.APIModels;

namespace EatIn.Models.API_models
{
    public class API
    {
        string key = "76610cf2dbmsh5f55412f62d30b9p17e3a5jsn26cd30ec4209";
        string host = "spoonacular-recipe-food-nutrition-v1.p.rapidapi.com";
        string baseUrl = " https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/";

        HttpClient client = new HttpClient();

        public List<API_recipe> Search(IEnumerable<Ingredient> ingredients, int numOfResults, int minOrMax, bool ignorePantry)
        {
            string endpoint = "recipes/findByIngredients?";

            var ingredientTitles = ingredients.Select(i => i.Title);
            string titlesJoined = string.Join(",", ingredientTitles);

            var emptyQuery = HttpUtility.ParseQueryString("");

            emptyQuery.Add("ingredients", titlesJoined);
            emptyQuery.Add("number", numOfResults.ToString());
            emptyQuery.Add("ranking", minOrMax.ToString());
            emptyQuery.Add("ignorePantry", ignorePantry.ToString());

            string url = baseUrl + endpoint + emptyQuery.ToString();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("X-RapidAPI-Host", host);
            request.Headers.Add("X-RapidAPI-Key", key);

            var response = client.SendAsync(request).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            var recipeList = JsonConvert.DeserializeObject<List<API_recipe>>(result);

            return recipeList;
        }

        public API_recipe_details GetDetails(int id, bool includeNutrition)
        {
            string endpoint1 = "recipes/";
            string endpoint2 = "/information";

            string url = baseUrl + endpoint1 + id.ToString() + endpoint2;

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Add("X-RapidAPI-Host", host);
            request.Headers.Add("X-RapidAPI-Key", key);

            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            var recipeDetails = JsonConvert.DeserializeObject<API_recipe_details>(result);

            return recipeDetails;
        }
    }
}