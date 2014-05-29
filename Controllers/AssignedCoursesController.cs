using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OctsProjectMvcTest.Models;

namespace OctsProjectMvcTest.Controllers
{ 
    public class AssignedCoursesController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /AssignedCourses/

        public ViewResult Index()
        {
            var assignedcoursedbset = db.AssignedCourseDbSet.Include(a => a.Course).Include(a => a.Teacher);
            return View(assignedcoursedbset.ToList());
        }

        //
        // GET: /AssignedCourses/Details/5

        public ViewResult Details(int id)
        {
            AssignedCourse assignedcourse = db.AssignedCourseDbSet.Find(id);
            return View(assignedcourse);
        }

        //
        // GET: /AssignedCourses/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.TeacherID = new SelectList("", "TeacherID", "TeacherName");
            return View();
        } 

        //
        // POST: /AssignedCourses/Create

        [HttpPost]
        public ActionResult Create(AssignedCourse assignedcourse)
        {
            if (ModelState.IsValid)
            {
                if(assignedcourse.CourseID==0||assignedcourse.TeacherID==0)
                {
                    ViewBag.Message = "All the fields are required !!!";
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
                    ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
                    ViewBag.TeacherID = new SelectList("", "TeacherID", "TeacherName");
                    return View();
                }

                AssignedCourse testAsnCourse = db.AssignedCourseDbSet.FirstOrDefault(asc => asc.CourseID == assignedcourse.CourseID);
                Course aCourse = db.CourseDbSet.FirstOrDefault(c => c.CourseID == assignedcourse.CourseID);
                Teacher aTeacher = db.TeacherDbSet.FirstOrDefault(t => t.TeacherID == assignedcourse.TeacherID);

                if (testAsnCourse != null)
                {

                    Teacher assignTeacher =
                        db.TeacherDbSet.FirstOrDefault(asignt => asignt.TeacherID == testAsnCourse.TeacherID);
                    if (assignTeacher != null)
                        if (aCourse != null)
                            ViewBag.Message = "The course :- " + aCourse.CourseCode + " has already been assaigned to "+assignTeacher.TeacherName;
                    ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
                    ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c=>c.DepartmentID==aCourse.DepartmentID), "CourseID", "CourseCode");
                    ViewBag.TeacherID = new SelectList(db.TeacherDbSet.Where(t=>t.DepartmentID==aTeacher.DepartmentID), "TeacherID", "TeacherName");
                    return View();
                }

                if (aTeacher.CreditsRemaining < aCourse.Credit)
                {
                    Session["Teacher"] = aTeacher;
                    Session["Course"] = aCourse;
                    Session["AssignedCourse"] = assignedcourse;
                    return RedirectToAction("AskToAssign");
                }
               
                aTeacher.CreditsHaveTaken += aCourse.Credit;
                aTeacher.CreditsRemaining -= aCourse.Credit;
                db.AssignedCourseDbSet.Add(assignedcourse);
                aCourse.AssignedTo = aTeacher.TeacherName;
                if (db.SaveChanges() >= 0)
                    ViewBag.Message = "The Course :- " + aCourse.CourseCode
                        + " has been assigned to Teacher :- " + aTeacher.TeacherName
                        + " successfully .";
                ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
                ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
                ViewBag.TeacherID = new SelectList("", "TeacherID", "TeacherName");
                return View();  
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.CourseID = new SelectList("", "CourseID", "CourseCode");
            ViewBag.TeacherID = new SelectList("", "TeacherID", "TeacherName");
            return View();
        }

        public ActionResult AskToAssign()
        {
            Teacher aTeacher = (Teacher)Session["Teacher"];
            Course aCourse = (Course)Session["Course"];
            ViewBag.Message = "Teacher :- " + aTeacher.TeacherName
                + " has only " + aTeacher.CreditsRemaining
                + " credits remaining . But, the credit of Course : " + aCourse.CourseCode
                + " is " + aCourse.Credit
                + "  ! Are you sure you want to assign this course to this teacher ?";
            return View();
        }

        public ActionResult AssignConfirmed()
        {
            Teacher aTeacher = (Teacher)Session["Teacher"];
            Course aCourse = (Course)Session["Course"];
            AssignedCourse anAssignedCourse = (AssignedCourse)Session["AssignedCourse"];
            Teacher teacher = db.TeacherDbSet.Find(aTeacher.TeacherID);
            Course course = db.CourseDbSet.Find(aCourse.CourseID);
            teacher.CreditsHaveTaken += aCourse.Credit;
            teacher.CreditsRemaining -= aCourse.Credit;
            db.AssignedCourseDbSet.Add(anAssignedCourse);
            course.AssignedTo = teacher.TeacherName;
            if (db.SaveChanges() > 0)
                ViewBag.Message = " The Course :- " + aCourse.CourseCode +
                    " has been assigned to Teacher :- " + aTeacher.TeacherName
                    + " successfully . Remaining Credits : " + teacher.CreditsRemaining;
            return View();
        }

        public ViewResult ViewCourseStatics()
        {
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            var coursedbset = db.CourseDbSet.Include(c => c.Department).Include(c => c.Semester);
            return View(coursedbset.ToList());
        }

        public PartialViewResult CourseFilter(int? departmentID)
        {
            List<Course> model;
            if (departmentID!=null)
            {
                model = db.CourseDbSet.Include(c=>c.Semester).Where(c => c.DepartmentID == departmentID).ToList();
            }
            else
            {
                model = db.CourseDbSet.Include(c=>c.Semester).ToList();
            }
            return PartialView("~/Views/AssignedCourses/_CourseFilter.cshtml",model);
        }

        public PartialViewResult LoadTeacherDropDown(int? departmentID)
        {
            if(departmentID!=null)
                ViewBag.TeacherID = new SelectList(db.TeacherDbSet.Where(t => t.DepartmentID == departmentID), "TeacherID", "TeacherName");
            return PartialView("~/Views/AssignedCourses/_LoadTeacherDropDown.cshtml");
        }

        public PartialViewResult LoadCourseDropDown(int? departmentID)
        {
            if (departmentID != null)
                ViewBag.CourseID = new SelectList(db.CourseDbSet.Where(c => c.DepartmentID == departmentID), "CourseID", "CourseCode");
            return PartialView("~/Views/AssignedCourses/_LoadCourseDropDown.cshtml");
        }

        public ActionResult TeacherInfo(int? teacherID)
        {
            Teacher teacherModel = db.TeacherDbSet.FirstOrDefault(t=>t.TeacherID==teacherID);
            return PartialView("~/Views/AssignedCourses/_TeacherInfo.cshtml",teacherModel);
        }

        public ActionResult CourseInfo(int? courseID)
        {
            Course courseModel = db.CourseDbSet.FirstOrDefault(c=>c.CourseID==courseID);
            return PartialView("~/Views/AssignedCourses/_CourseInfo.cshtml", courseModel);
        }

        //
        // GET: /AssignedCourses/Edit/5
 
        public ActionResult Edit(int id)
        {
            AssignedCourse assignedcourse = db.AssignedCourseDbSet.Find(id);
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", assignedcourse.CourseID);
            ViewBag.TeacherID = new SelectList(db.TeacherDbSet, "TeacherID", "TeacherName", assignedcourse.TeacherID);
            return View(assignedcourse);
        }

        //
        // POST: /AssignedCourses/Edit/5

        [HttpPost]
        public ActionResult Edit(AssignedCourse assignedcourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignedcourse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.CourseDbSet, "CourseID", "CourseCode", assignedcourse.CourseID);
            ViewBag.TeacherID = new SelectList(db.TeacherDbSet, "TeacherID", "TeacherName", assignedcourse.TeacherID);
            return View(assignedcourse);
        }

        //
        // GET: /AssignedCourses/Delete/5
 
        public ActionResult Delete(int id)
        {
            AssignedCourse assignedcourse = db.AssignedCourseDbSet.Find(id);
            return View(assignedcourse);
        }

        //
        // POST: /AssignedCourses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            AssignedCourse assignedcourse = db.AssignedCourseDbSet.Find(id);
            Course aCourse = db.CourseDbSet.FirstOrDefault(c => c.CourseID == assignedcourse.CourseID);
            Teacher aTeacher = db.TeacherDbSet.FirstOrDefault(t => t.TeacherID == assignedcourse.TeacherID);
            aTeacher.CreditsHaveTaken -= aCourse.Credit;
            aTeacher.CreditsRemaining += aCourse.Credit;
            db.AssignedCourseDbSet.Remove(assignedcourse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}