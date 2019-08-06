using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EatIn.Models.ViewModels
{
    public class IngredientVM
    {
        public string Title { get; set; }
        public double Amount { get; set; }

        public int MeasurementId { get; set; }

    }
}