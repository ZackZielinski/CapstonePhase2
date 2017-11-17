using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Collections.Generic;
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
            var AttendingStudent = db.Students.SingleOrDefault(z=>z.Id == studentid);

            var PriorStudent = db.Students_Lectures.Include(x=>x.Lecture).Include(y=>y.Student).SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (PriorStudent == null)
            {
                PriorStudent = NewStudentInLecture(studentid, lectureid);
            }

            if (AttendingStudent != null)
            {
                AttendingStudent.Lectureid = lectureid;
            }
            db.SaveChanges();
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
            db.SaveChanges();
            return View(StudentAnswers);
        }

        [HttpPost]
        public ActionResult CodeAssignment(int lectureId, int studentId, string CodeText)
        {
            var StudentCodeAnswers = db.Students_Lectures.SingleOrDefault(x => x.LectureId == lectureId && x.StudentId == studentId);

            return RedirectToAction("Compiler", new { filename = "" });
        }

        public ActionResult Compiler(string filename)
        {
            string CodeFile = System.IO.File.ReadAllText($@"{ filename }");
            var results = CompileCsharpSource(new[] { CodeFile }, "App.exe");

            var  CompilerFeedback = InsertErrors(results.Errors);

            return View(CompilerFeedback);
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

                var NewLecture = db.Lectures.SingleOrDefault(x=>x.Id == lectures.Id);

                return RedirectToAction("CreateNewTest", new { lectureid = NewLecture.Id });
            }

            return View(lectures);
        }

        public ActionResult CreateNewTest(int lectureid)
        {
            var Lecture = db.Lectures.Find(lectureid);

            if (Lecture.CodeFileName == null || Lecture.CodeFileName == "")
            {
                Lecture.CodeFileName = GenerateNewTestFile(Lecture.Topic);
                Lecture.CodeFileText = GetFileText(Lecture.CodeFileName);
            }
            db.SaveChanges();

            return View(Lecture);
        }

        [HttpPost]
        public ActionResult CreateNewTest(Lectures lecture)
        {
            var LectureInDB = db.Lectures.Find(lecture.Id);

            LectureInDB.CodeFileText = lecture.CodeFileText;
            db.SaveChanges();
            UpdateTestCodeFile(LectureInDB.CodeFileName, LectureInDB.CodeFileText);

            List<string> Lines = System.IO.File.ReadAllLines(LectureInDB.CodeFileName).ToList();

            List<string> LinesWithoutMainMethod = FilterOutMainMethod(Lines);

            LectureInDB.ListOfMethodNames = StartFindingMethods(LinesWithoutMainMethod);
            
            db.SaveChanges();
            return RedirectToAction("ConfigureMethods", new { id = LectureInDB.Id });
        }

        public ActionResult ConfigureMethods(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var Lecture = db.Lectures.Find(id);

            if(Lecture == null)
            {
                return HttpNotFound();
            }
            
            foreach(var MethodName in Lecture.ListOfMethodNames)
            {
                Lecture.MethodsAndReturnValues.Add(MethodName, null);
            }

            db.SaveChanges();

            return View(Lecture);
        }

        [HttpPost]
        public ActionResult ConfiureMethods(Lectures Lecture)
        {
            var LectureinDB = db.Lectures.Find(Lecture.Id);

            LectureinDB.MethodsAndReturnValues = Lecture.MethodsAndReturnValues;

            db.SaveChanges();

            return RedirectToAction("Index");
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

        public static List<string> FilterOutMainMethod(List<string> LinesInFile)
        {
            List<string> LinesWithoutMainMethod = new List<string>();
            int UnfinishedBrackets = 0;

            foreach (var line in LinesInFile)
            {
                if (line.Contains('{'))
                {
                    UnfinishedBrackets--;
                }

                if (line.Contains('}'))
                {
                    UnfinishedBrackets++;
                }

                if (UnfinishedBrackets == 0)
                {
                    LinesWithoutMainMethod.Add(line);
                }
            }

            return LinesWithoutMainMethod;
        }


        protected List<string> StartFindingMethods(List<string> LinesinFile)
        {
            List<string> MethodList = new List<string>();
            string Method;

            foreach (var line in LinesinFile)
            {
                Method = FindMethod(line);
                if (Method != "" || Method != null)
                {
                    MethodList.Add(Method);
                }
            }
            return MethodList;
        }

        protected string FindMethod(string LineText)
        {
            string MethodName = "";

            for (int x = 1; x < LineText.Count(); x++)
            {
                if (LineText.ElementAt(x - 1) == '.')
                {
                    int y = x;
                    while (LineText.ElementAt(y) != ')')
                    {
                        MethodName += LineText.ElementAt(y);
                        y++;
                    }
                    MethodName += ")";
                    break;
                }
            }

            return MethodName;
        }


        protected void UpdateTestCodeFile(string FileName, string FileText)
        {
            var TestCodeFile = new StreamWriter(FileName);
            TestCodeFile.WriteLine(FileText);
            TestCodeFile.Close();
        }


        protected string GetFileText(string FileName)
        {
            string FileText = System.IO.File.ReadAllText(FileName);

            return FileText;
        }

        protected string GenerateNewTestFile(string LectureTopic)
        {
            string TestCodeFilePath = @"C:\Users\Zack\Desktop\C# (Sharp)\CapStonePhase2\CapStonePhase2\TestCode";

            string NewFilePath = $@"{ TestCodeFilePath }\{LectureTopic}Test.cs";

            StreamWriter NewFile = new StreamWriter($@"{NewFilePath}");
            string NewLine = "\r\n";
            NewFile.WriteLine($"using System;{NewLine}");
            NewFile.WriteLine($"public class Program {{{NewLine}");
            NewFile.WriteLine($"static void Main(){{ {NewLine}");
            NewFile.WriteLine($"{NewLine} }} }}");
            NewFile.Close();

            return NewFilePath;
        }

        protected Students_Lectures InsertErrors(CompilerErrorCollection results)
        {
            var CurrentUser = User.Identity.GetUserId();
            var StudentInDB = db.Students.SingleOrDefault(y => y.Userid == CurrentUser);
            var Student_LectureInDB = db.Students_Lectures.Include(x => x.Student).SingleOrDefault(y => y.StudentId == StudentInDB.Id && y.LectureId == StudentInDB.Lectureid);

            Student_LectureInDB.ListOfErrors = results;
            Student_LectureInDB.NumberOfErrors = results.Count;
            db.SaveChanges();

            for (int x = results.Count - 1; x >= 0; x--)
            {
                var error = results[x];

                if (error.ErrorNumber == "CS1567" || error.ErrorNumber == "CS1610")
                {
                    Student_LectureInDB.NumberOfErrors--;
                    Student_LectureInDB.ListOfErrors.Remove(error);
                    continue;
                }

                if (error.IsWarning == true)
                {
                    Student_LectureInDB.ListOfWarnings.Add(error);
                    Student_LectureInDB.NumberOfErrors--;
                }
            }
            Student_LectureInDB.NumberOfWarnings = Student_LectureInDB.ListOfWarnings.Count;
            db.SaveChanges();

            CheckIfStudentPassed(Student_LectureInDB);

            return Student_LectureInDB;
        }

        protected void CheckIfStudentPassed(Students_Lectures AnsweredStudent)
        {
            var StudentInDB = db.Students_Lectures.SingleOrDefault(z => z.StudentId == AnsweredStudent.StudentId && z.LectureId == AnsweredStudent.LectureId);

            if (StudentInDB.NumberOfErrors == 0)
            {
                StudentInDB.IsCodeCorrect = true;
            }

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
            AttendingStudent.Lectureid = lectureid;

            Students_Lectures StudentInLecture = new Students_Lectures()
            {
                Student = AttendingStudent,
                Lecture = SelectedLecture,
            };

            db.Students_Lectures.Add(StudentInLecture);
            AttendingStudent.Lectureid = lectureid;
            db.SaveChanges();
            return StudentInLecture;
        }

        protected static CompilerResults CompileCsharpSource(string[] sources, string output, params string[] references)
        {
                        
            var parameters = new CompilerParameters(references, output)
            {
                GenerateExecutable = true
            };
            
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
