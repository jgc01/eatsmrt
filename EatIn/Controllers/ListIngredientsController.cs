using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using EatIn.Models;
using Microsoft.AspNet.Identity;

namespace EatIn.Controllers
{
    public class ListIngredientsController : Controller
    {
        private EatInDBEntities db = new EatInDBEntities();

        // GET: ListIngredients
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var listIngredients = db.ListIngredients.Where(i => i.UserProfileId == userId);

            var ingredientsByRecipe = listIngredients.GroupBy(i => i.RecipeTitle);
            ViewBag.byRecipe = ingredientsByRecipe;

            return View(listIngredients.ToList());
        }

        public ActionResult GroceryListPartial(string id)
        {
            string userId;
            if (!String.IsNullOrEmpty(id) && Request.IsLocal)
            {
                userId = id;
            }
            else
            {
                userId = User.Identity.GetUserId();
            }
            var listIngredients = db.ListIngredients.Where(i => i.UserProfileId == userId);
            return PartialView(listIngredients.ToList());
        }

        [HttpPost]
        public ActionResult EmailGroceryList()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(Request.Url.GetLeftPart(UriPartial.Authority) + Url.Action("GroceryListPartial", new { id = User.Identity.GetUserId() })).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var message = @"<!DOCTYPE html>
<html><head><link rel=""stylesheet"" type=""text/css"" href=""https://localhost:44370/Content/Site.css""></head><body>" + responseBody + "</body></html>";
            EmailUtility.SendConfirmation(User.Identity.GetUserName(), message, $"eat smrt: Shopping List For {DateTime.Now.ToString("dddd, dd MMMM yyyy")}", true);
            return new HttpStatusCodeResult(HttpStatusCode.NoContent);
        }

        // GET: ListIngredients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListIngredient listIngredient = db.ListIngredients.Find(id);
            if (listIngredient == null)
            {
                return HttpNotFound();
            }
            return View(listIngredient);
        }

        // GET: ListIngredients/Create
        public ActionResult Create()
        {
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName");
            return View();
        }

        // POST: ListIngredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Amount,Name,Unit,RecipeTitle,RecipeUrl")] ListIngredient listIngredient)
        {
            if (ModelState.IsValid)
            {
                listIngredient.UserProfileId = User.Identity.GetUserId();

                db.ListIngredients.Add(listIngredient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(listIngredient);
        }
        //Get: Batch Create
        public ActionResult CreateMany()
        {
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName");
            return View();
        }

        //Post: Batch Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMany(IEnumerable<ListIngredient> listIngredients)
        {
            if (ModelState.IsValid)
            {
                foreach (var listIngredient in listIngredients)
                {
                    listIngredient.UserProfileId = User.Identity.GetUserId();
                }
                db.ListIngredients.AddRange(listIngredients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(listIngredients);
        }

        // GET: ListIngredients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListIngredient listIngredient = db.ListIngredients.Find(id);
            if (listIngredient == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName", listIngredient.UserProfileId);
            return View(listIngredient);
        }

        // POST: ListIngredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserProfileId,Amount,Name,Unit,RecipeTitle,RecipeUrl")] ListIngredient listIngredient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listIngredient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName", listIngredient.UserProfileId);
            return View(listIngredient);
        }

        // GET: ListIngredients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListIngredient listIngredient = db.ListIngredients.Find(id);
            if (listIngredient == null)
            {
                return HttpNotFound();
            }
            return View(listIngredient);
        }

        // POST: ListIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListIngredient listIngredient = db.ListIngredients.Find(id);
            db.ListIngredients.Remove(listIngredient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteMany")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMany(IEnumerable<ListIngredient> listIngredients)
        {
            var userId = User.Identity.GetUserId();
            listIngredients = db.ListIngredients.Where(i => i.UserProfileId == userId);
            db.ListIngredients.RemoveRange(listIngredients);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
