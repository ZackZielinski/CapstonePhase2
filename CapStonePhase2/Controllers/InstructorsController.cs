using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;

namespace CapStonePhase2.Controllers
{
    public class InstructorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Instructors
        public ActionResult Index()
        {
            return View(db.Instructors.ToList());
        }

        public ActionResult GradeShortAnswer(int studentid, int lectureid)
        {
            var StudentAnswers = db.Students_Lectures.Include(x=>x.Lecture).Include(y=>y.Student).SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (StudentAnswers == null)
            {
                return HttpNotFound();
            }

            return View(StudentAnswers);
        }

        public ActionResult ChangeGrade(int studentid, int lectureid)
        {
            var GradingStudent = db.Students_Lectures.SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (GradingStudent == null)
            {
                return HttpNotFound();
            }

            if(GradingStudent.IsShortAnswerCorrect == false)
            {
                GradingStudent.IsShortAnswerCorrect = true;
            }
            else
            {
                GradingStudent.IsShortAnswerCorrect = false;
            }

            db.SaveChanges();

            CheckIfStudentPassed(GradingStudent);
            return RedirectToAction("GradeShortAnswer", new { studentid = studentid, lectureid = lectureid });
        }

        // GET: Instructors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructors instructors = db.Instructors.Find(id);
            if (instructors == null)
            {
                return HttpNotFound();
            }
            return View(instructors);
        }

        // GET: Instructors/Create
        public ActionResult Create()
        {
            Instructors NewInstructor = new Instructors();
            return View(NewInstructor);
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instructors instructors)
        {
            if (instructors.Id == 0)
            {
                db.Instructors.Add(instructors);
                db.SaveChanges();
                return RedirectToAction("Index", "Students");
            }

            return View(instructors);
        }

        // GET: Instructors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructors instructors = db.Instructors.Find(id);
            if (instructors == null)
            {
                return HttpNotFound();
            }
            return View(instructors);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instructors instructors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructors).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Students");
            }
            return View(instructors);
        }

        // GET: Instructors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructors instructors = db.Instructors.Find(id);
            if (instructors == null)
            {
                return HttpNotFound();
            }

            db.Instructors.Remove(instructors);
            db.SaveChanges();
            return RedirectToAction("Index", "Students");
        }


        protected void CheckIfStudentPassed(Students_Lectures Student)
        {
            var AttendedStudent = db.Students_Lectures.SingleOrDefault(z => z.StudentId == Student.StudentId && z.LectureId == Student.LectureId);

            if (AttendedStudent.IsCodeCorrect == true && AttendedStudent.IsShortAnswerCorrect == true)
            {
                AttendedStudent.CompletedCourse = true;
            }
            else
            {
                AttendedStudent.CompletedCourse = false;
            }
            db.SaveChanges();
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
