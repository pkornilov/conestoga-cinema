using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using conestogacinema_project.Models;
using IMDb_Scraper;

namespace conestogacinema_project.Controllers
{
    public class MoviesController : Controller
    {
        private ConestogaCinemaContext db = new ConestogaCinemaContext();

        // GET: Movies
        public ActionResult Index()
        {
            var movies = db.movies.Include(m => m.genre);
            return View(movies.OrderBy(m => m.movie_title).ToList());
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // GET: Movies/Create
        [Authorize]
        public ActionResult Create(string imdb = "")
        {
            ViewBag.genre_id = new SelectList(db.genres, "genre_id", "genre_name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "movie_id,movie_title,movie_description,movie_runtime,movie_release_date,genre_id,imdb_id,imdb_poster")] movy movy)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["successMessage"] = "Movie has been successfully added!";
                    db.movies.Add(movy);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["errorMessage"] = "This movie is already in the library";
                }
            }

            ViewBag.genre_id = new SelectList(db.genres, "genre_id", "genre_name", movy.genre_id);
            return View(movy);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            ViewBag.genre_id = new SelectList(db.genres, "genre_id", "genre_name", movy.genre_id);
            return View(movy);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "movie_id,movie_title,movie_description,movie_runtime,movie_release_date,genre_id,imdb_id,imdb_poster")] movy movy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.genre_id = new SelectList(db.genres, "genre_id", "genre_name", movy.genre_id);
            return View(movy);
        }

        // GET: Movies/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            movy movy = db.movies.Find(id);
            if (movy == null)
            {
                return HttpNotFound();
            }
            return View(movy);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            movy movy = db.movies.Find(id);
            db.movies.Remove(movy);
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
