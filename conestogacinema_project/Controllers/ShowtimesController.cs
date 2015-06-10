using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using conestogacinema_project.Models;
using System.Collections;
using System.Globalization;

namespace conestogacinema_project.Controllers
{
    public class ShowtimesController : Controller
    {
        private ConestogaCinemaContext db = new ConestogaCinemaContext();

        // GET: Showtimes
        public ActionResult Index()
        {
            var showtimes = db.showtimes.Include(s => s.AspNetUser).Include(s => s.movy).Include(s => s.room);
            return View(showtimes.Where(s => s.showtime_date >= DateTime.Now).OrderBy(s => s.showtime_date).ToList());
        }

        // GET: Showtimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            showtime showtime = db.showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            

            return View(showtime);
        }

        // GET: Showtimes/Create
        [Authorize]
        public ActionResult Create(string room = "", string movie = "")
        {
            var dates = new List<DateTime>();
            for (var dt = DateTime.Now; dt < DateTime.Now.AddDays(21); dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            ViewBag.showtime_date = new SelectList(
                dates.Select(
                    c => new
                    {
                        date = c.ToString("dddd, MMMM d, yyyy"),
                    }),
                    "date",
                    "date");


            List<String> times = new List<String>();
            TimeSpan startTime = new TimeSpan(18, 0, 0);            
            DateTime startDate = new DateTime(DateTime.MinValue.Ticks);
            for (int i = 0; i < 12; i++)
            {
                int minutesToBeAdded = 30 * i;
                TimeSpan timeToBeAdded = new TimeSpan(0, minutesToBeAdded, 0);
                TimeSpan t = startTime.Add(timeToBeAdded);
                DateTime result = startDate + t;
                times.Add(result.ToShortTimeString());               
            }

            ViewBag.showtime_time = new SelectList(
                times.Select(
                    c => new
                    {
                        time = c
                    }),
                    "time",
                    "time");


            ViewBag.host_student_id = new SelectList(
                db.AspNetUsers.Select(
                    c => new
                    {
                        Id = c.Id,
                        FullName = c.FirstName + " " + c.LastName
                    }),
                    "Id",
                    "FullName",
                    User.Identity.GetUserId());


            ViewBag.movie_id = new SelectList(
                db.movies.OrderBy(x=>x.movie_title).Select(
                    c => new
                    {
                        movie_id = c.movie_id,
                        movie_title_year = c.movie_title + " (" + c.movie_release_date.ToString().Substring(0, 4) + ")"
                    }),
                    "movie_id",
                    "movie_title_year",
                    movie);


            ViewBag.room_id = new SelectList(
                db.rooms.Select(
                    c => new
                    {
                        room_id = c.room_id,
                        room_title_seats = c.room_title + " (" + c.room_seats + " seats)"
                    }),
                    "room_id",
                    "room_title_seats",
                    room);

            return View();
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "showtime_id,showtime_date,movie_id,room_id,host_student_id")] showtime showtime, string showtime_time)
        {
            DateTime timeRaw = DateTime.ParseExact(showtime_time, "h:mm tt", CultureInfo.InvariantCulture);
            TimeSpan time = timeRaw.TimeOfDay;

            showtime.showtime_date += time;

            if (ModelState.IsValid)
            {
                db.showtimes.Add(showtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.host_student_id = new SelectList(db.AspNetUsers, "Id", "Email", showtime.host_student_id);
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_title", showtime.movie_id);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_title", showtime.room_id);
            return View(showtime);
        }

        // GET: Showtimes/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            showtime showtime = db.showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            ViewBag.host_student_id = new SelectList(db.AspNetUsers, "Id", "Email", showtime.host_student_id);
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_title", showtime.movie_id);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_title", showtime.room_id);
            return View(showtime);
        }

        // POST: Showtimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "showtime_id,showtime_date,movie_id,room_id,host_student_id")] showtime showtime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(showtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.host_student_id = new SelectList(db.AspNetUsers, "Id", "Email", showtime.host_student_id);
            ViewBag.movie_id = new SelectList(db.movies, "movie_id", "movie_title", showtime.movie_id);
            ViewBag.room_id = new SelectList(db.rooms, "room_id", "room_title", showtime.room_id);
            return View(showtime);
        }

        // GET: Showtimes/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            showtime showtime = db.showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            showtime showtime = db.showtimes.Find(id);
            db.showtimes.Remove(showtime);
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
