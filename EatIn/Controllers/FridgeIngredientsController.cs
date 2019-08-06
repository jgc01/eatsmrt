using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EatIn.Models;
using EatIn.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace EatIn.Controllers
{
    public class FridgeIngredientsController : Controller
    {
        private EatInDBEntities db = new EatInDBEntities();

        // GET: FridgeIngredients
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var fridgeIngredients = db.FridgeIngredients.Where(i => i.UserProfileId == userId);
            return View(fridgeIngredients.OrderBy(i => i.Ingredient.Title).ToList());
        }

        // GET: FridgeIngredients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FridgeIngredient fridgeIngredient = db.FridgeIngredients.Find(id);
            if (fridgeIngredient == null)
            {
                return HttpNotFound();
            }
            return View(fridgeIngredient);
        }

        // GET: FridgeIngredients/Create
        [ChildActionOnly]
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IngredientId = new SelectList(db.Ingredients, "Id", "Title");
            ViewBag.MeasurementId = new SelectList(db.Measurements, "Id", "unit");
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName");
            return View();
        }

        // POST: FridgeIngredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(IngredientVM viewModel)
        {
            var userId = User.Identity.GetUserId();



            if (ModelState.IsValid)
            {
                var ingredient = db.Ingredients.SingleOrDefault(i => i.Title == viewModel.Title);
                if (ingredient == null)
                {
                    ingredient = new Ingredient
                    {
                        Title = viewModel.Title.ToLower(),
                    };
                }

                var fridgeIngredient = db.FridgeIngredients.SingleOrDefault(i => i.IngredientId == ingredient.Id && i.UserProfileId == userId && i.MeasurementId == viewModel.MeasurementId);
                if (fridgeIngredient == null)
                {
                    fridgeIngredient = new FridgeIngredient
                    {
                        Ingredient = ingredient,
                        Amount = viewModel.Amount,
                        MeasurementId = viewModel.MeasurementId,
                        UserProfileId = User.Identity.GetUserId(),
                    };

                    db.FridgeIngredients.Add(fridgeIngredient);
                }
                else
                {
                    fridgeIngredient.Amount += viewModel.Amount;
                    db.Entry(fridgeIngredient).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IngredientId = new SelectList(db.Ingredients, "Id", "Title");
            ViewBag.MeasurementId = new SelectList(db.Measurements, "Id", "unit", viewModel.MeasurementId);
            return View(viewModel);
        }

        // GET: FridgeIngredients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FridgeIngredient fridgeIngredient = db.FridgeIngredients.Find(id);
            if (fridgeIngredient == null)
            {
                return HttpNotFound();
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "Id", "Title", fridgeIngredient.IngredientId);
            ViewBag.MeasurementId = new SelectList(db.Measurements, "Id", "unit", fridgeIngredient.MeasurementId);
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName", fridgeIngredient.UserProfileId);
            return View(fridgeIngredient);
        }

        // POST: FridgeIngredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserProfileId,IngredientId,Amount,MeasurementId")] FridgeIngredient fridgeIngredient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fridgeIngredient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "Id", "Title", fridgeIngredient.IngredientId);
            ViewBag.MeasurementId = new SelectList(db.Measurements, "Id", "unit", fridgeIngredient.MeasurementId);
            ViewBag.UserProfileId = new SelectList(db.UserProfiles, "UserId", "FirstName", fridgeIngredient.UserProfileId);
            return View(fridgeIngredient);
        }

        // GET: FridgeIngredients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FridgeIngredient fridgeIngredient = db.FridgeIngredients.Find(id);
            if (fridgeIngredient == null)
            {
                return HttpNotFound();
            }
            return View(fridgeIngredient);
        }

        // POST: FridgeIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FridgeIngredient fridgeIngredient = db.FridgeIngredients.Find(id);
            db.FridgeIngredients.Remove(fridgeIngredient);
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
