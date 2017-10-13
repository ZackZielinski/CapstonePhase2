using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapStonePhase2.Models;

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
            var StudentInLecture = db.Students_Lectures.Include(z=>z.Student.Id == studentid && z.Lecture.Id == lectureid).SingleOrDefault();
            return View(StudentInLecture);
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
            return View();
        }

        // POST: Lectures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Topic,Description,ReviewQuestion,CodeAssignment,StudentId")] Lectures lectures)
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Topic,Description,ReviewQuestion,CodeAssignment,StudentId")] Lectures lectures)
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
            return View(lectures);
        }

        // POST: Lectures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lectures lectures = db.Lectures.Find(id);
            db.Lectures.Remove(lectures);
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
