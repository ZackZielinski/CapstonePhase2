using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;
using Microsoft.AspNet.Identity;

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

        public ActionResult Lectures()
        {
            var LectureList = db.Lectures.ToList();

            var CurrentUser = User.Identity.GetUserId();
            var PriorStudent = db.Students.Include(y => y.Usertype).SingleOrDefault(z=>z.Userid == CurrentUser);

            foreach(var Lecture in LectureList)
            {
                Lecture.StudentId = PriorStudent.Id;
            }

            return View(LectureList);
        }

        public ActionResult CompletedCourses(int studentid)
        {
            var StudentCourses = db.Students_Lectures.Include(x=>x.Student).Include(y=>y.Lecture).Where(z => z.StudentId == studentid).ToList();

            if(StudentCourses == null)
            {
                return HttpNotFound();
            }

            return View(StudentCourses);
        }

        public ActionResult ViewCompletedCourses()
        {
            string CurrentUser = User.Identity.GetUserId();
            var CurrentStudent = db.Students.Include(y => y.Usertype).SingleOrDefault(z => z.Userid == CurrentUser);
            var StudentCourses = db.Students_Lectures.Include(y=>y.Lecture).Where(z => z.StudentId == CurrentStudent.Id).ToList();

            if (StudentCourses == null)
            {
                return HttpNotFound();
            }

            return View(StudentCourses);
        }


        public ActionResult Grade(int studentid, int lectureid)
        {
            var StudentProgress = db.Students_Lectures.SingleOrDefault(z=>z.StudentId == studentid && z.LectureId == lectureid);

            if(StudentProgress == null)
            {
               StudentProgress = NewStudentInLecture(studentid, lectureid);
            }

            return View(StudentProgress);
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
                students.Userid = User.Identity.GetUserId();
                db.Students.Add(students);
                db.SaveChanges();
                return RedirectToAction("Lectures");
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
                return RedirectToAction("Lectures", new { studentid = students.Id } );
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
            return RedirectToAction("Index");
        }

        protected Students_Lectures NewStudentInLecture(int studentid, int lectureid)
        {
            var AttendedStudent = db.Students.SingleOrDefault(y => y.Id == studentid);
            var SelectedLecture = db.Lectures.SingleOrDefault(z => z.Id == lectureid);

            Students_Lectures NewStudent = new Students_Lectures()
            {
                Lecture = SelectedLecture,
                Student = AttendedStudent
            };
            db.Students_Lectures.Add(NewStudent);
            AttendedStudent.Lectureid = lectureid;
            db.SaveChanges();

            return NewStudent;
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
