using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using conestogacinema_project.Models;

namespace conestogacinema_project.Controllers
{
    public class RoomsController : Controller
    {
        private ConestogaCinemaContext db = new ConestogaCinemaContext();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.rooms.Include(r => r.room_types);
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.type_id = new SelectList(db.room_types, "type_id", "type_name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "room_id,room_title,room_seats,room_projector,type_id")] room room)
        {
            if (ModelState.IsValid)
            {
                db.rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.type_id = new SelectList(db.room_types, "type_id", "type_name", room.type_id);
            return View(room);
        }

        // GET: Rooms/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.type_id = new SelectList(db.room_types, "type_id", "type_name", room.type_id);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "room_id,room_title,room_seats,room_projector,type_id")] room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.type_id = new SelectList(db.room_types, "type_id", "type_name", room.type_id);
            return View(room);
        }

        // GET: Rooms/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            room room = db.rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            room room = db.rooms.Find(id);
            db.rooms.Remove(room);
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
