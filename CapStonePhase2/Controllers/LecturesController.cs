using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Web;

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
            var AttendingStudent = db.Students.SingleOrDefault(z=>z.Id == studentid);
            var SelectedLecture = db.Lectures.SingleOrDefault(z=>z.Id == lectureid);

            Students_Lectures StudentInLecture = new Students_Lectures()
            {
                Student = AttendingStudent,
                Lecture = SelectedLecture
            };
            return View(StudentInLecture);
        }


        public ActionResult ReviewQuestion(int studentid, int lectureid)
        {
            var AnsweredStudent = db.Students_Lectures.SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (AnsweredStudent == null)
            {
                return HttpNotFound();
            }

            return View(AnsweredStudent);
        }

        // POST: ReviewQuestion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewQuestion(Students_Lectures StudentAnswers)
        {
            var EnteredStudent = db.Students_Lectures.Find(StudentAnswers);

            EnteredStudent.ShortAnswer = StudentAnswers.ShortAnswer;

            db.SaveChanges();
            return RedirectToAction("CodeAssignment", new { studentid = StudentAnswers.StudentId, lectureid = StudentAnswers.LectureId });
        }

        public ActionResult CodeAssignment(int studentid, int lectureid)
        {
            var StudentAnswers = db.Students_Lectures.SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (StudentAnswers == null)
            {
                return HttpNotFound();
            }

            return View(StudentAnswers);
        }

        public ActionResult EnterAssignment(int studentid, int lectureid, HttpPostedFileBase CodeFile)
        {
            var StudentAnswer = db.Students_Lectures.SingleOrDefault(z=>z.StudentId == studentid && z.LectureId == lectureid);

            var NewFile = Server.MapPath("~/CodeData/" + CodeFile.FileName);

            if (CodeFile.ContentLength > 0)
            {
                CodeFile.SaveAs(NewFile);
                StudentAnswer.CodeFileName = NewFile;
                db.SaveChanges();
                return RedirectToAction("Compiler", new { student = StudentAnswer });
            }

            return RedirectToAction("ReviewQuestion", new { studentid = studentid, lectureid = lectureid });
            
        }

        private ActionResult Compiler(Students_Lectures StudentAnswers)
        {
            string CodeFile = System.IO.File.ReadAllText($@"{StudentAnswers.CodeFileName}");
            var results = CompileCsharpSource(new[] { CodeFile }, "App.exe");

            if(results.Errors.Count != 0)
            {
                StudentAnswers.ListOfErrors = results.Errors;
                StudentAnswers.NumberOfErrors = results.Errors.Count;
                StudentAnswers.IsCodeCorrect = false;
                db.SaveChanges();
                return View(StudentAnswers);
            }
            else if (results.Errors.Count == 0 && StudentAnswers.IsCodeCorrect == false)
            {
                StudentAnswers.IsCodeCorrect = true;
                StudentAnswers.NumberOfErrors = results.Errors.Count;
                db.SaveChanges();
            }

            if (StudentAnswers.IsCodeCorrect == true && StudentAnswers.IsShortAnswerCorrect == true)
            {
                StudentAnswers.CompletedCourse = true;
                db.SaveChanges();
            }
            return View(StudentAnswers);
        }

        private static CompilerResults CompileCsharpSource(string[] sources, string output, params string[] references)
        {
            var parameters = new CompilerParameters(references, output);
            parameters.GenerateExecutable = true;
            using (var provider = new CSharpCodeProvider())
                return provider.CompileAssemblyFromSource(parameters, sources);
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
                return RedirectToAction("Index", "Students");
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
            return RedirectToAction("Index", "Students");
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
