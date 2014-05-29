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
    public class CoursesController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /Courses/

        public ViewResult Index()
        {
            var coursedbset = db.CourseDbSet.Include(c => c.Department).Include(c => c.Semester);
            return View(coursedbset.ToList());
        }

        //
        // GET: /Courses/Details/5

        public ViewResult Details(int id)
        {
            Course course = db.CourseDbSet.Find(id);
            return View(course);
        }

        //
        // GET: /Courses/Create

        public ActionResult Create()
        {
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName");
            return View();
        } 

        //
        // POST: /Courses/Create

        [HttpPost]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.AssignedTo = "Yet Not Assigned ";
                db.CourseDbSet.Add(course);
                if (db.SaveChanges() > 0)
                    ViewBag.Message = "Course :- " + course.CourseName
                        + " has been saved successfully .";
                ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
                ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
                return View(course);  
            }

            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
            return View(course);
        }
        
        //
        // GET: /Courses/Edit/5
 
        public ActionResult Edit(int id)
        {
            Course course = db.CourseDbSet.Find(id);
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
            return View(course);
        }

        //
        // POST: /Courses/Edit/5

        [HttpPost]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", course.DepartmentID);
            ViewBag.SemesterID = new SelectList(db.SemesterDbSet, "SemesterID", "SemesterName", course.SemesterID);
            return View(course);
        }

        //
        // GET: /Courses/Delete/5
 
        public ActionResult Delete(int id)
        {
            Course course = db.CourseDbSet.Find(id);
            return View(course);
        }

        //
        // POST: /Courses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Course course = db.CourseDbSet.Find(id);
            db.CourseDbSet.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Check_CourseCode(string courseCode)
        {
            var result = db.CourseDbSet.Count(c => c.CourseCode == courseCode) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Check_CourseName(string courseName)
        {
            var result = db.CourseDbSet.Count(c => c.CourseName == courseName) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}