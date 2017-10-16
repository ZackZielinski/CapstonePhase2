﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;

namespace CapStonePhase2.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        public ActionResult Lectures(int studentid)
        {
            var LectureList = db.Lectures.ToList();

            foreach(var Lecture in LectureList)
            {
                Lecture.StudentId = studentid;
            }

            return View(LectureList);
        }

        public ActionResult CompletedCourses(int studentid)
        {
            var StudentCourses = db.Students_Lectures.Where(z => z.StudentId == studentid).ToList();

            return View(StudentCourses);
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }



        // GET: Students/Create
        public ActionResult Create()
        {
            Students NewStudent = new Students();
            return View(NewStudent);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Students students)
        {
            if (students.Id == 0)
            {
                db.Students.Add(students);
                db.SaveChanges();
                var NewStudent = db.Students.Last();
                return RedirectToAction("Lectures", new { studentid = NewStudent.Id });
            }
            return View(students);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }
            return View(students);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Students students)
        {
            if (ModelState.IsValid)
            {
                db.Entry(students).State = EntityState.Modified;
                db.SaveChanges();
                var ChangedStudent = db.Students.Find(students.Id);
                return RedirectToAction("Lectures", new { studentid = ChangedStudent.Id } );
            }
            return View(students);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students students = db.Students.Find(id);
            if (students == null)
            {
                return HttpNotFound();
            }

            db.Students.Remove(students);
            db.SaveChanges();
            return RedirectToAction("Lectures");
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