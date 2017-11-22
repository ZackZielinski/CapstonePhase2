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
            var StudentAnswers = db.Students_Lectures.Include(x => x.Lecture).Include(y => y.Student).SingleOrDefault(z => z.StudentId == studentid && z.LectureId == lectureid);

            if (StudentAnswers == null)
            {
                StudentAnswers = NewStudentInLecture(studentid, lectureid);
            }

            StudentAnswers.CodeFileName = CreateNewStudentCodeFile(StudentAnswers);
            StudentAnswers.MethodsAndReturnValues = db.Methods.Where(x => x.Lectureid == lectureid).ToList();

            if(StudentAnswers.CodeFileText == null || StudentAnswers.CodeFileText == "")
            {
                StudentAnswers.CodeFileText = CreateMethodTemplate();
            }
            
            db.SaveChanges();
            return View(StudentAnswers);
        }

        [HttpPost]
        public ActionResult CodeAssignment(int lectureId, int studentId, string CodeFileText)
        {
            var StudentCodeAnswers = db.Students_Lectures.Include(x => x.Lecture).SingleOrDefault(x => x.LectureId == lectureId && x.StudentId == studentId);

            InsertStudentCode(StudentCodeAnswers, CodeFileText);

            return RedirectToAction("Compiler", new { filename = StudentCodeAnswers.CodeFileName });
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
 
            UpdateCodeFile(LectureInDB.CodeFileName, LectureInDB.CodeFileText);


            List<string> Lines = System.IO.File.ReadAllLines(LectureInDB.CodeFileName).ToList();

            List<string> LinesWithoutMainMethod = FilterOutMainMethod(Lines);

            List<string> ListOfMethodNames = StartFindingMethods(LinesWithoutMainMethod);


            InsertNewMethods(ListOfMethodNames, LectureInDB);

            db.SaveChanges();
            return RedirectToAction("ConfigureMethods", new { id = LectureInDB.Id });
        }

        public ActionResult ConfigureMethods(int id)
        {
            var MethodsInLectureTest = db.Methods.Where(x => x.Lectureid == id).ToList();

            if (MethodsInLectureTest == null)
            {
                return HttpNotFound();
            }

            return View(MethodsInLectureTest);
        }

        [HttpPost]
        public ActionResult ConfigureMethods(List<Methods> UpdatedMethods)
        {
            var FirstMethod = UpdatedMethods.FirstOrDefault();

            if(FirstMethod == null)
            {
                return HttpNotFound();
            }

            var MethodsInDB = db.Methods.Where(x => x.Lectureid == FirstMethod.Lectureid).ToList();

            for (int index = 0; index < UpdatedMethods.Count(); index++)
            {
                if(MethodsInDB[index].Id == UpdatedMethods[index].Id)
                {
                    MethodsInDB[index].ReturnValueType = UpdatedMethods[index].ReturnValueType;
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }

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

        public ActionResult LectureTest(int id)
        {
            var Lecture = db.Lectures.Find(id);

            if(Lecture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(Lecture);
        }

        [HttpPost]
        public ActionResult LectureTest(Lectures UpdatedTestFile)
        {
            var LectureinDB = db.Lectures.Find(UpdatedTestFile.Id);

            LectureinDB.CodeFileText = UpdatedTestFile.CodeFileText;
            db.SaveChanges();
            
            UpdateCodeFile(LectureinDB.CodeFileName, LectureinDB.CodeFileText);

            return RedirectToAction("Index");
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

        protected void InsertStudentCode(Students_Lectures StudentCode, string CodeFileText)
        {
            var LectureTest = db.Lectures.Find(StudentCode.LectureId);
            List<string> MainMethod = GetMainMethod(LectureTest.CodeFileName);

            UpdateCodeFile(StudentCode.CodeFileName, CodeFileText);
            EnterStudentCode(MainMethod, StudentCode.CodeFileName);
            InsertInstructorTests(LectureTest.CodeFileName, StudentCode.CodeFileName);
        }

        protected void InsertInstructorTests(string InstructorFileName, string StudentFileName)
        {
            List<string> AllInstructorText = System.IO.File.ReadAllLines(InstructorFileName).ToList();
            List<string> FliteredInstructorText = FilterOutMainMethod(AllInstructorText);
            List<string> InstructorTestMethods = RemoveSystemInCode(FliteredInstructorText);

            StreamWriter StudentFile = new StreamWriter(StudentFileName, true);
            StudentFile.WriteLine('}');

            foreach(var line in InstructorTestMethods)
            {
                StudentFile.Write(line);
            }
            StudentFile.Close();
        }

        protected List<string> RemoveSystemInCode(List<string> InstructorCodeText)
        {
            List<string> AllTests = new List<string>();
            bool FirstLine = true;
            int semicolons = 0;

            foreach (var line in InstructorCodeText)
            {
                if (line.Contains(";") && semicolons == 0)
                {
                    semicolons++;
                    FirstLine = false;
                    continue;
                }

                if (FirstLine == false)
                {
                    AllTests.Add(line);
                }
            }


            return AllTests;
        }

        protected void EnterStudentCode(List<string> MainMethodCode, string StudentCodeFileName)
        {
            List<string> StudentCode = System.IO.File.ReadAllLines(StudentCodeFileName).ToList();
            StreamWriter CodeFile = new StreamWriter(StudentCodeFileName);
            foreach(var line in MainMethodCode)
            {
                CodeFile.WriteLine(line);
            }

            foreach(var line in StudentCode)
            {
                CodeFile.WriteLine(line);
            }
            CodeFile.Close();
        }

        public static List<string> GetMainMethod(string FileName)
        {
            List<string> LinesInFile = System.IO.File.ReadAllLines(FileName).ToList();
            List<string> EndingMainMethod = new List<string>();
            int EndingBrackets = 0;
            int UnfinishedBrackets = 1;
            bool EndOfMainMethod = false;

            foreach (var line in LinesInFile)
            {
                if (line.Contains('{'))
                {
                    UnfinishedBrackets -= FindAllStartingBrackets(line);
                }
                if (line.Contains('}'))
                {
                    EndingBrackets += FindAllEndingBrackets(line);
                    UnfinishedBrackets += EndingBrackets;
                }

                if (UnfinishedBrackets == 0)
                {
                    EndOfMainMethod = true;
                }

                if (UnfinishedBrackets == 1 || EndOfMainMethod == true)
                {
                    EndingMainMethod.Add(line);
                }

                if (EndingBrackets == 1 && EndOfMainMethod == true)
                {
                    break;
                }

            }
            return EndingMainMethod;
        }

        protected void InsertNewMethods(List<string> MethodNames, Lectures Lecture)
        {
            foreach (var method in MethodNames)
            {
                Methods NewMethod = new Methods()
                {
                    MethodName = method,
                    AssociatedLecture = Lecture,
                    Lectureid = Lecture.Id
                };
                db.Methods.Add(NewMethod);
            }

            db.SaveChanges();
        }

        public static List<string> FilterOutMainMethod(List<string> LinesInFile)
        {
            List<string> LinesWithoutMainMethod = new List<string>();
            int UnfinishedBrackets = 0;
            int EndingBrackets = 0;
            bool EndOfMainMethod = false;


            foreach (var line in LinesInFile)
            {
                if (line.Contains('{'))
                {
                    EndingBrackets = 0;
                    UnfinishedBrackets += FindAllStartingBrackets(line);
                }

                if (line.Contains('}'))
                {
                    EndingBrackets += FindAllEndingBrackets(line);
                    UnfinishedBrackets -= EndingBrackets;
                }

                if (UnfinishedBrackets == 0 && EndOfMainMethod == false)
                {
                    LinesWithoutMainMethod.Add(line);
                }

                if (EndingBrackets == 2 && EndOfMainMethod == false)
                {
                    EndOfMainMethod = true;
                    continue;
                }

                if (EndOfMainMethod == true)
                {
                    LinesWithoutMainMethod.Add(line);
                }
            }

            return LinesWithoutMainMethod;
        }

        protected static int FindAllStartingBrackets(string line)
        {
            int StartingBracketsfound = 0;

            foreach (var character in line)
            {
                if (character == '{')
                {
                    StartingBracketsfound++;
                }
            }

            return StartingBracketsfound;
        }

        protected static int FindAllEndingBrackets(string line)
        {
            int EndingBracketsfound = 0;

            foreach (var character in line)
            {
                if (character == '}')
                {
                    EndingBracketsfound++;
                }
            }

            return EndingBracketsfound;
        }

        protected List<string> StartFindingMethods(List<string> LinesInFile)
        {
            List<string> MethodList = new List<string>();
            string Method = "";

            foreach (var line in LinesInFile)
            {

                if (line.Contains("Console"))
                {
                    continue;
                }

                Method = FindMethod(line);
                if (Method != "" && Method != null)
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


        protected void UpdateCodeFile(string FileName, string FileText)
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

        protected string CreateNewStudentCodeFile(Students_Lectures AttendingStudent)
        {
            string NewLine = "\r\n";
            string StudentCodeFilePath = @"C:\Users\Zack\Desktop\C# (Sharp)\CapStonePhase2\CapStonePhase2\StudentCode";

            string NewFilePath = $@"{ StudentCodeFilePath }\{AttendingStudent.Student.FirstName}{AttendingStudent.Student.LastName}{AttendingStudent.Lecture.Topic}.cs";

            string MethodTemplate = $"public datatype MethodName() {{ {NewLine} return //insert matching datatype value; {NewLine} }}";

            StreamWriter NewFile = new StreamWriter($@"{NewFilePath}");
            NewFile.WriteLine($"{MethodTemplate}");
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

        protected string CreateMethodTemplate()
        {
            string NewLine = "\r\n";
            string MethodTemplate = $"public datatype MethodName() {{ {NewLine} return //insert matching datatype value; {NewLine} }}";

            return MethodTemplate;
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
