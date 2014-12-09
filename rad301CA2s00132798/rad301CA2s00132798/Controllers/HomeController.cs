using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using rad301CA2s00132798.Models;

namespace rad301CA2s00132798.Controllers
{
    public class HomeController : Controller
    {
        MoviesDB db = new MoviesDB();
        //
        // GET: /Home/

        public ActionResult Index(string sortOrder)
        {
            ViewBag.PageTitle = "List of Movies (Total of " + db.Actors.Count() + " actors)";
            if (sortOrder == null) sortOrder = "ascNumber";
            ViewBag.numberOrder = (sortOrder == "ascNumber") ? "descNumber" : "ascNumber";
            ViewBag.dateOrder = (sortOrder == "ascDate") ? "descDate" : "ascDate";

            IQueryable<Movie> movies = db.Movies;
            switch (sortOrder)
            {
                case "descDate":
                    ViewBag.dateOrder = "ascDate";
                    movies = movies.OrderByDescending(m => m.ReleaseDate).Include("MovieActors");
                    break;
                case "descNumber":
                    ViewBag.numberOrder = "ascNumber";
                    movies = movies.OrderByDescending(m => m.MovieActors.Count).Include("MovieActors");
                    break;
                case "ascDate":
                    ViewBag.dateOrder = "descDate";
                    movies = movies.OrderBy(m => m.ReleaseDate).Include(m => m.MovieActors);
                    break;
                case "ascNumber":
                    ViewBag.numberOrder = "descNumber";
                    movies = movies.OrderBy(m => m.MovieActors.Count).Include("MovieActors");
                    break;
                default:
                    ViewBag.numberOrder = "ascNumber";
                    movies = movies.OrderBy(m => m.MovieActors.Count).Include("MovieActors");
                    break;
            }
            return View(movies.ToList());
        }

        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var q = db.Movies.Find(id);
            if (q == null)
            {
                Debug.WriteLine("Record not found");
                ViewBag.PageTitle = String.Format("Sorry, record {0} not found.", id);
            }
            else
            {
                ViewBag.PageTitle = "Details of " + q.Title + " (" + ((q.MovieActors.Count == 0) ? "None" : q.MovieActors.Count.ToString()) + ')';
                ViewBag.SexStatsMale = q.MovieActors.Count(act => act.Sex == Sex.Male);
                ViewBag.SexStatsFemale = q.MovieActors.Count(act => act.Sex == Sex.Female);
            }

            return View(q);
        }

        public PartialViewResult ActorsById(int id)
        {
            var movie = db.Movies.Find(id);
            @ViewBag.movieId = id;
            @ViewBag.movieName = movie.Title;
            return PartialView("_ActorsOnMovie", movie.MovieActors);
        }

        //
        // GET: /Home/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.directorList = db.Directors.ToList();
            return View();
        }

        //
        // POST: /Home/Create
        [HttpPost]
        public ActionResult Create(Movie incomingMovie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new MoviesDB())
                    {
                        db.Movies.Add(incomingMovie);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public PartialViewResult CreateActor(Actor incomingActor)
        {
            if (ModelState.IsValid)
            {
                db.Actors.Add(incomingActor);
                db.SaveChanges();
                return ActorsById(incomingActor.MovieId);
            }
            return null;
        }

        //
        // GET: /Home/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.directorList = db.Directors.ToList();
            return View(db.Movies.Find(id));
        }

        //
        // POST: /Home/Edit/5
        [HttpPost]
        public ActionResult Edit(Movie editMovie)
        {
            try
            {
                db.Entry(editMovie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public HttpStatusCodeResult UpdateActor(Actor actor)
        {
            try
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        //
        // GET: /Home/Delete/5
        public ActionResult Delete(int id)
        {
            return View(db.Movies.Find(id));
        }

        //
        // POST: /Home/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Movies.Remove(db.Movies.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("DeleteActor")]
        public PartialViewResult DeleteActor(int id, int movieId)
        {
            db.Actors.Remove(db.Actors.Find(id));
            db.SaveChanges();
            return PartialView("_ActorsOnMovie", db.Movies.Find(movieId).MovieActors);
        }
    }
}
