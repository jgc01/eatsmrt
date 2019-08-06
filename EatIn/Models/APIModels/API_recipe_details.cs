using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatIn.Models.APIModels
{
    public class API_recipe_details
    {
        public bool vegetarian { get; set; }
        public bool vegan { get; set; }
        public bool glutenFree { get; set; }
        public bool dairyFree { get; set; }
        public double servings { get; set; }
        public string sourceUrl { get; set; }
        public string apiUrl { get; set; }

    }
}