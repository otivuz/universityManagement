using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OctsProjectMvcTest.Models;
using RazorPDF;
using iTextSharp;
using iTextSharp.text;

namespace OctsProjectMvcTest.Controllers
{ 
    public class EnrollmentsController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /Enrollments/

        public ViewResult Index()
        {
            var enrollmentdbset = db.EnrollmentDbSet.Include(e => e.Student).Include(e => e.Course).Include(e => e.Grade);
            return View(enrollmentdbset.ToList());
        }

        //
        // GET: /Enrollments/Details/5

        public ViewResult Details(int id)
        {
            Enrollment enrollment = db.EnrollmentDbSet.Find(id);
            return View(enrollment);
        }

        //
        // GET: /Enrollments/Create

        public ActionResult Create()
        {
            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo");
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode");
            ViewBag.GradeID = new SelectList(db.GradeDbSet, "GradeID", "GradeLetter");
            return View();
        } 

        //
        // POST: /Enrollments/Create

        [HttpPost]
        public ActionResult Create(Enrollment enrollment)
        {
            Student student = db.StudentDbSet.Find(enrollment.StudentID);
            ViewBag.Name = student.StudentName;
            ViewBag.Email = student.Email;
            ViewBag.Dept = student.Department.DeptName;
            if (ModelState.IsValid)
            {
                Enrollment testEnrollment
                    = db.EnrollmentDbSet.FirstOrDefault(
                        e => (e.StudentID == enrollment.StudentID) && (e.CourseID == enrollment.CourseID));

                Course course
                    = db.CourseDbSet.Find(enrollment.CourseID);
                if (testEnrollment==null)
                {
                    enrollment.GradeID = 1;
                    db.EnrollmentDbSet.Add(enrollment);
                    if (db.SaveChanges() > 0)
                        ViewBag.Message = "This Course : " + course.CourseCode + " has been successfully Enrolled to student " + student.StudentName;
                    ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo", enrollment.StudentID);
                    ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", enrollment.CourseID);
                    ViewBag.GradeID = new SelectList(db.GradeDbSet, "GradeID", "GradeLetter", enrollment.GradeID);
                    return View(enrollment);                   
                }
                else
                {
                    ViewBag.Message = "This Course : " + course.CourseCode + " is already Enrolled to student " + student.StudentName;
                    ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo", enrollment.StudentID);
                    ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", enrollment.CourseID);
                    ViewBag.GradeID = new SelectList(db.GradeDbSet, "GradeID", "GradeLetter", enrollment.GradeID);
                    return View(enrollment);
                }
  
            }

            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo", enrollment.StudentID);
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", enrollment.CourseID);
            ViewBag.GradeID = new SelectList(db.GradeDbSet, "GradeID", "GradeLetter", enrollment.GradeID);
            return View(enrollment);
        }

        public ActionResult ResultEntry()
        {
            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.GradeID = new SelectList(db.GradeDbSet.Where(g=>g.GradeID!=1), "GradeID", "GradeLetter");
            return View();
        }

        [HttpPost]
        public ActionResult ResultEntry(Enrollment enrollment)
        {

            if (ModelState.IsValid)
            {
                Enrollment anEnrollment = db.EnrollmentDbSet.FirstOrDefault(e=>(e.CourseID==enrollment.CourseID)&&(e.StudentID==enrollment.StudentID));
                Student student
                    = db.StudentDbSet.Find(enrollment.StudentID);
                Course course
                    = db.CourseDbSet.Find(enrollment.CourseID);
                if (anEnrollment.GradeID == 1)
                {
                    anEnrollment.GradeID = enrollment.GradeID;
                    if (db.SaveChanges() > 0)
                        ViewBag.Message = "Result For This Course : " + course.CourseCode +
                                          " has been successfully Entered For student " + student.StudentName;
                    ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo");
                    ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
                    ViewBag.GradeID = new SelectList(db.GradeDbSet.Where(g => g.GradeID != 1), "GradeID", "GradeLetter");
                    return View();
                }

                ViewBag.Message = "Result For This Course : " + course.CourseCode 
                    + " has already been published For student " + student.StudentName;
                ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo");
                ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
                ViewBag.GradeID = new SelectList(db.GradeDbSet.Where(g => g.GradeID != 1), "GradeID", "GradeLetter");
                return View();
                
            }
            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.GradeID = new SelectList(db.GradeDbSet.Where(g => g.GradeID != 1), "GradeID", "GradeLetter");
            return View();
        }

        public ActionResult ViewResult()
        {
            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo");
            
            return View();
        }
        
        [HttpPost]
        public ActionResult ViewResult(int? studentID)
        {
            return new PdfResult(db.EnrollmentDbSet.Where(e=>e.StudentID==studentID).ToList(),"ViewInPDF");
        }

        public PartialViewResult ResultInfo(int? studentID)
        {
            List<Enrollment> model = new List<Enrollment>();

            if (studentID!=null)
            {
                model =
                    db.EnrollmentDbSet.Include(e => e.Course)
                      .Include(e => e.Grade)
                      .Where(e => e.StudentID == studentID)
                      .ToList();
            }

            else
            {
                model =
                    db.EnrollmentDbSet.Include(e => e.Course)
                      .Include(e => e.Grade)
                      .ToList();
            }

            if (model.Count()==0)
            {
                Student student = db.StudentDbSet.Find(studentID);
                ViewBag.NotEnrolled = "Student :- " + student.StudentName + " with Reg No. " + student.RegNo
                    + " has not taken any course for this semester .";
            }

            return PartialView("~/Views/Enrollments/_ResultInfo.cshtml", model);
        }

        //
        // GET: /Enrollments/Edit/5
 
        public ActionResult Edit(int id)
        {
            Enrollment enrollment = db.EnrollmentDbSet.Find(id);
            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo", enrollment.StudentID);
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", enrollment.CourseID);
            ViewBag.GradeID = new SelectList(db.GradeDbSet, "GradeID", "GradeLetter", enrollment.GradeID);
            return View(enrollment);
        }

        //
        // POST: /Enrollments/Edit/5

        [HttpPost]
        public ActionResult Edit(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentID = new SelectList(db.StudentDbSet, "StudentID", "RegNo", enrollment.StudentID);
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", enrollment.CourseID);
            ViewBag.GradeID = new SelectList(db.GradeDbSet, "GradeID", "GradeLetter", enrollment.GradeID);
            return View(enrollment);
        }

        //
        // GET: /Enrollments/Delete/5
 
        public ActionResult Delete(int id)
        {
            Enrollment enrollment = db.EnrollmentDbSet.Find(id);
            return View(enrollment);
        }

        //
        // POST: /Enrollments/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Enrollment enrollment = db.EnrollmentDbSet.Find(id);
            db.EnrollmentDbSet.Remove(enrollment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public PartialViewResult StudentInfo(int? studentID)
        {
            if (studentID != null)
            {
                Student student = db.StudentDbSet.FirstOrDefault(s => s.StudentID == studentID);
                ViewBag.Name = student.StudentName;
                ViewBag.Email = student.Email;
                ViewBag.Dept = student.Department.DeptName;
            }
            return PartialView("~/Views/Enrollments/_StudentInfo.cshtml");
        }

        public PartialViewResult FillCourseDropDown(int? studentID)
        {
            List<Enrollment> enrollments=new List<Enrollment>();
            List<Course> courses =new List<Course>();
            if (studentID!=null)
            {
                enrollments = db.EnrollmentDbSet.Where(e => e.StudentID == studentID).ToList();
                foreach (Enrollment anEnrollment in enrollments)
                {
                    Course aCourse = db.CourseDbSet.FirstOrDefault(c => c.CourseID == anEnrollment.CourseID);
                    courses.Add(aCourse);
                }
                ViewBag.CourseID = new SelectList(courses, "CourseID", "CourseCode");
            }
            return PartialView("~/Views/Enrollments/_FillCourseDropDown.cshtml");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}