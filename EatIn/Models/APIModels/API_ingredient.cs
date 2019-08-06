using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatIn.Models.API_models
{
    public class API_ingredient
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public double amount { get; set; }
        public string unit { get; set; }
    }
}