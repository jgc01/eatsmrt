using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatIn.Models.API_models
{
    public class API_recipe
    {
        public int id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public List<API_ingredient> missedIngredients { get; set; }
        public List<API_ingredient> usedIngredients { get; set; }
    }

}