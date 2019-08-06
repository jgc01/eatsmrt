using EatIn.Models;
using EatIn.Models.API_models;
using EatIn.Models.APIModels;
using EatIn.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EatIn.Controllers
{
    public class SearchController : Controller
    {
        private EatInDBEntities db = new EatInDBEntities();
       
        // GET: Search
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var api = new API();

            var ingredients = db.FridgeIngredients.Where(i => i.UserProfileId == userId).Select(i => i.Ingredient);

            var recipes = api.Search(ingredients, 25, 2, true);

            var recipesWithDetails = recipes.Select(i => new API_RecipeVM(i, api)).ToList();

            return View(recipesWithDetails);
        }

        // GET: Search/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var api = new API();
            var recipeDetails = api.GetDetails(id, false);
            return Redirect(recipeDetails.sourceUrl);
        }

        //// GET: Search/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Search/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Search/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Search/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Search/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Search/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
