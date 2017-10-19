using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Web;
using System;

namespace CapStonePhase2.Controllers
{
    public class LecturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lectures
        public ActionResult Index()
        {
            return View(db.Lectures.ToList());
        }

        public ActionResult Description(int studentid, int lectureid)
        {
            var PriorStudent = db.Students_Lectures.Include(x=>x.Lecture).Include(y=>y.Student).SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (PriorStudent == null)
            {
                PriorStudent = NewStudentInLecture(studentid, lectureid);
            }
            return View(PriorStudent);
        }


        public ActionResult ReviewQuestion(int studentid, int lectureid)
        {
            var AnsweredStudent = db.Students_Lectures.Include(y=>y.Lecture).SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (AnsweredStudent == null)
            {
                AnsweredStudent = NewStudentInLecture(studentid, lectureid);
            }

            return View(AnsweredStudent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewQuestion(Students_Lectures StudentAnswers)
        {
            var EnteredStudent = db.Students_Lectures.SingleOrDefault(z=>z.StudentId == StudentAnswers.StudentId && z.LectureId == StudentAnswers.LectureId);

            if(EnteredStudent == null)
            {
                return HttpNotFound();
            }

            EnteredStudent.ShortAnswer = StudentAnswers.ShortAnswer;

            db.SaveChanges();
            return RedirectToAction("CodeAssignment", new { studentid = EnteredStudent.StudentId, lectureid = EnteredStudent.LectureId });
        }

        
        public ActionResult CodeAssignment(int studentid, int lectureid)
        {
            var StudentAnswers = db.Students_Lectures.Include(x=>x.Lecture).SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (StudentAnswers == null)
            {
                StudentAnswers = NewStudentInLecture(studentid, lectureid);
            }

            return View(StudentAnswers);
        }

        [HttpPost]
        public ActionResult CodeAssignment(HttpPostedFileBase CodeFile, Students_Lectures Student)
        {
            var StudentAnswer = db.Students_Lectures.SingleOrDefault(z => z.StudentId == Student.StudentId && z.LectureId == Student.LectureId);

            var NewFile = Server.MapPath("~/CodeData/" + CodeFile.FileName);

            if (CodeFile.ContentLength > 0)
            {
                CodeFile.SaveAs(NewFile);
                StudentAnswer.CodeFileName = NewFile;
                db.SaveChanges();
                return RedirectToAction("Compiler", new { filename = NewFile });
            }

            if (StudentAnswer.ShortAnswer == null)
            {
                return RedirectToAction("ReviewQuestion", new { studentid = StudentAnswer.StudentId, lectureid = StudentAnswer.LectureId });
            }

            return RedirectToAction("Lectures", "Students", new { studentid = StudentAnswer.StudentId });
        }

        private ActionResult Compiler(Students_Lectures StudentAnswers)
        {
            string CodeFile = System.IO.File.ReadAllText($@"{ StudentAnswers.CodeFileName }");
            var results = CompileCsharpSource(new[] { CodeFile }, "App.exe");


           
            StudentAnswers.ListOfErrors = results.Errors;
            StudentAnswers.NumberOfErrors = results.Errors.Count;
            db.SaveChanges();

            CheckIfStudentPassed(StudentAnswers);

            return View(StudentAnswers);
        }

        // GET: Lectures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lectures lectures = db.Lectures.Find(id);
            if (lectures == null)
            {
                return HttpNotFound();
            }
            return View(lectures);
        }

        // GET: Lectures/Create
        public ActionResult Create()
        {
            Lectures NewLecture = new Lectures();
            return View(NewLecture);
        }

        // POST: Lectures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lectures lectures)
        {
            if (ModelState.IsValid)
            {
                db.Lectures.Add(lectures);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lectures);
        }

        // GET: Lectures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lectures lectures = db.Lectures.Find(id);
            if (lectures == null)
            {
                return HttpNotFound();
            }
            return View(lectures);
        }

        // POST: Lectures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lectures lectures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lectures).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lectures);
        }

        // GET: Lectures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lectures lectures = db.Lectures.Find(id);
            if (lectures == null)
            {
                return HttpNotFound();
            }
            db.Lectures.Remove(lectures);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected void CheckIfStudentPassed(Students_Lectures AnsweredStudent)
        {
            var StudentInDB = db.Students_Lectures.SingleOrDefault(z => z.StudentId == AnsweredStudent.StudentId && z.LectureId == AnsweredStudent.LectureId);

            if (StudentInDB.NumberOfErrors == 0)
            {
                StudentInDB.IsCodeCorrect = true;
            }
            db.SaveChanges();

            if (StudentInDB.IsCodeCorrect == true && StudentInDB.IsShortAnswerCorrect == true)
            {
                StudentInDB.CompletedCourse = true;
            }
            else
            {
                StudentInDB.CompletedCourse = false;
            }
            db.SaveChanges();
        }


        protected Students_Lectures NewStudentInLecture(int studentid, int lectureid)
        {
            var AttendingStudent = db.Students.SingleOrDefault(z => z.Id == studentid);
            var SelectedLecture = db.Lectures.SingleOrDefault(z => z.Id == lectureid);

            Students_Lectures StudentInLecture = new Students_Lectures()
            {
                Student = AttendingStudent,
                Lecture = SelectedLecture
            };

            db.Students_Lectures.Add(StudentInLecture);
            db.SaveChanges();
            return StudentInLecture;
        }

        protected static CompilerResults CompileCsharpSource(string[] sources, string output, params string[] references)
        {
            var parameters = new CompilerParameters(references, output);
            parameters.GenerateExecutable = true;
            using (var provider = new CSharpCodeProvider())
                return provider.CompileAssemblyFromSource(parameters, sources);
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
