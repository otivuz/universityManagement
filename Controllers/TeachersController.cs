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
    public class TeachersController : Controller
    {
        private RootProjDbContext db = new RootProjDbContext();

        //
        // GET: /Teachers/

        public ViewResult Index()
        {
            var teacherdbset = db.TeacherDbSet.Include(t => t.Designation).Include(t => t.Department);
            return View(teacherdbset.ToList());
        }

        //
        // GET: /Teachers/Details/5

        public ViewResult Details(int id)
        {
            Teacher teacher = db.TeacherDbSet.Find(id);
            return View(teacher);
        }

        //
        // GET: /Teachers/Create

        public ActionResult Create()
        {
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName");
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode");
            return View();
        } 

        //
        // POST: /Teachers/Create

        [HttpPost]
        public ActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.CreditsHaveTaken = 0.0000;
                teacher.CreditsRemaining = teacher.CreditsToBeTaken;
                db.TeacherDbSet.Add(teacher);
                if (db.SaveChanges() > 0)
                    ViewBag.Message = "Teacher :- " + teacher.TeacherName
                        + " has been saved successfully";
                ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
                ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
                return View(teacher);   
            }

            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
            return View(teacher);
        }
        
        //
        // GET: /Teachers/Edit/5
 
        public ActionResult Edit(int id)
        {
            Teacher teacher = db.TeacherDbSet.Find(id);
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
            return View(teacher);
        }

        //
        // POST: /Teachers/Edit/5

        [HttpPost]
        public ActionResult Edit(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignationID = new SelectList(db.DesignationDbSet, "DesignationID", "DsgName", teacher.DesignationID);
            ViewBag.DepartmentID = new SelectList(db.DepartmentDbSet, "DepartmentID", "DeptCode", teacher.DepartmentID);
            return View(teacher);
        }

        //
        // GET: /Teachers/Delete/5
 
        public ActionResult Delete(int id)
        {
            Teacher teacher = db.TeacherDbSet.Find(id);
            return View(teacher);
        }

        //
        // POST: /Teachers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Teacher teacher = db.TeacherDbSet.Find(id);
            db.TeacherDbSet.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Check_Email(string email)
        {
            var result = db.TeacherDbSet.Count(t => t.Email == email) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}